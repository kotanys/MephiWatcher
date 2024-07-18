using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using MephiWatcher.Parsers;

namespace MephiWatcher
{
    public sealed partial class MainForm : Form
    {
        private readonly TextBox _errorLabel = new()
        {
            Text = "Произошла ошибка. Если в конфигурационном файле всё верно, то возможно, сайт МИФИ изменился, и программа не знает, как его обрабатывать.",
            ReadOnly = true,
            BackColor = SystemColors.Control,
            WordWrap = true,
            Multiline = true,
        };

        private readonly TextBox _cancelLabel = new()
        {
            Text = "Операция отменена.",
            ReadOnly = true,
            BackColor = SystemColors.Control,
        };


        private readonly IConfigFactory _configFactory;
        private readonly Dictionary<string, IVuzParser> _parsers;

        private VuzConfig? CurrentVuzConfig
        {
            get
            {
                if (_chooseVuzComboBox.SelectedIndex == -1)
                    return null;
                return (VuzConfig)_chooseVuzComboBox.Items[_chooseVuzComboBox.SelectedIndex]!;
            }
        }

        private Config _config;
        private CancellationTokenSource? _cancellation;

        public MainForm(IConfigFactory configFactory, IEnumerable<IVuzParser> parsers)
        {
            InitializeComponent();
            _configFactory = configFactory;
            _parsers = parsers.ToDictionary(GetVuzParserName);
            _config = configFactory.Create();
            UpdateConfigInfo(true);
        }

        private static string GetVuzParserName(IVuzParser p)
        {
            return p.GetType().GetCustomAttribute<VuzParserNameAttribute>()?.Name ?? throw new InvalidOperationException($"VuzParser does not have a {nameof(VuzParserNameAttribute)}");
        }

#pragma warning disable IDE1006 // Events in this project are named precisely like {control}_{event}
        private async void _findButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in _programsFlowLayoutPanel.Controls)
            {
                if (control != _errorLabel && control != _cancelLabel)
                    control.Dispose();
            }
            _programsFlowLayoutPanel.Controls.Clear();

            if (CurrentVuzConfig is null)
            {
                return;
            }
            _findButton.Enabled = false;
            _cancellation = new CancellationTokenSource();
            try
            {
                var parser = _parsers[CurrentVuzConfig.Name.ToLowerInvariant()];
                var entries = ParseRatingWebsiteAsync(parser, _cancellation.Token);
                await foreach (var entry in entries)
                {
                    var box = CreateTextBox(entry);
                    _programsFlowLayoutPanel.Controls.Add(box);
                }
            }
            catch (OperationCanceledException)
            {
                _programsFlowLayoutPanel.Controls.Add(_cancelLabel);
            }
            catch (Exception ex)
            {
                ParseFail(ex);
#if DEBUG
                throw;
#endif
            }
            finally
            {
                _cancellation.Dispose();
                _cancellation = null;
                _findButton.Enabled = true;
            }
        }

        private void _programsFlowLayoutPanel_SizeChanged(object sender, EventArgs e)
        {
            ResizeTextBoxes();
        }

        private void _configButton_Click(object sender, EventArgs e)
        {
            var process = Process.Start("notepad", "config.json");
            process.WaitForExit();
            _config = _configFactory.Create();
            UpdateConfigInfo(true);
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            _cancellation?.Cancel();
        }

        private void _programsFlowLayoutPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is null)
                return;
            e.Control.Width = _programsFlowLayoutPanel.Width;
        }

        private void _chooseVuzComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _cancellation?.Cancel();
            UpdateConfigInfo(false);
        }
#pragma warning restore IDE1006

        private async IAsyncEnumerable<EntryDto> ParseRatingWebsiteAsync(IVuzParser parser, [EnumeratorCancellation] CancellationToken ct = default)
        {
            if (CurrentVuzConfig is null)
            {
                yield break;
            }
            var programs = (await parser.ParseProgramsAsync(CurrentVuzConfig.Url, ct)).Where(ProgramSatisfies);
            var ratings = programs.Select(async p => await parser.RarseProgramRatingAsync(p, ct));
            foreach (var task in ratings)
            {
                ct.ThrowIfCancellationRequested();

                var rating = await task;
                var totalCount = rating.Entries.Count();
                Entry? existing = rating.Entries.SingleOrDefault(IsMe);
                if (existing is not null)
                {
                    yield return new EntryDto(existing.SerialNumber, totalCount, rating.Program, Status.Pass);
                    continue;
                }

                int placement = FindMyPlacement(_config.TotalPoints, rating);
                var dto = new EntryDto(placement, totalCount, rating.Program, placement == -1 ? Status.Fail : Status.Possible);
                yield return dto;
            }
        }

        private bool IsMe(Entry rating)
        {
            return rating.Document == _config.Document;
        }

        private bool ProgramSatisfies(VuzProgram program)
        {
            //return program.Name.Contains("очная форма") && !program.Name.Contains("квота") && !program.Name.Contains("прием");
            return CurrentVuzConfig?.ProgramNames.Contains(program.Name.Trim()) ?? throw new InvalidOperationException("Vuz not selected");
        }

        private static TextBox CreateTextBox(EntryDto dto)
        {
            string placement = dto.Program.Name + "  Место: " +
                (dto.Status != Status.Fail ? (dto.SerialNumber.ToString() + "/" + dto.TotalNumber.ToString()) : "нет");
            var box = new TextBox
            {
                Text = placement,
                Multiline = true,
                ReadOnly = true,
                BackColor = MakeColor(dto),
            };
            box.Height *= 2;
            box.Click += (s, e) => new Process { StartInfo = { UseShellExecute = true, CreateNoWindow = true, FileName = "cmd.exe", Arguments = "/c start " + dto.Program.Url } }.Start();
            return box;
        }

        private void UpdateConfigInfo(bool updateVuzList)
        {
            if (updateVuzList)
            {
                _chooseVuzComboBox.Items.Clear();
                foreach (var vuzConfig in _config.VuzConfigs)
                {
                    _chooseVuzComboBox.Items.Add(vuzConfig);
                }
            }

            StringBuilder? builder = null;
            if (CurrentVuzConfig is not null)
            {
                builder = new();
                foreach (var program in CurrentVuzConfig.ProgramNames)
                {
                    builder.Append(program.Length >= 80 ? program[..80] : program);
                    builder.AppendLine(" ...");
                }
            }
            _configInfoTextBox.Text = $"""
                                      Баллы: {_config.TotalPoints}, Программы:
                                      {(builder is null ? "вуз не выбран" : builder)}
                                      """;
        }

        private void ParseFail(Exception ex)
        {
            var appdataDir = Environment.GetEnvironmentVariable("appdata")!;
            var appdataDirForThisApp = Path.Combine(appdataDir, "MephiWatcher");
            Directory.CreateDirectory(appdataDirForThisApp);
            File.WriteAllText(Path.Combine(appdataDirForThisApp, $"log {DateTime.Now:FFFFFFF}.txt"), ex.ToString());
            _errorLabel.Size = _programsFlowLayoutPanel.Size;
            _programsFlowLayoutPanel.Controls.Add(_errorLabel);
        }

        /// <summary>
        /// Находит место, на котором был бы абитуриент с <paramref name="totalPoints"/> в рейтинге <paramref name="rating"/>.
        /// </summary>
        /// <param name="totalPoints">Данное количество баллов.</param>
        /// <param name="rating">Рейтинговый список.</param>
        /// <returns>Место в рейтинге.</returns>
        private static int FindMyPlacement(int totalPoints, ProgramRating rating)
        {
            int i = 0;
            foreach (var entry in rating.Entries.OrderBy(e => e.SerialNumber))
            {
                i++;
                if (totalPoints >= entry.Points)
                    return i;
            }
            return -1;
        }

        private void ResizeTextBoxes()
        {
            foreach (Control control in _programsFlowLayoutPanel.Controls)
            {
                control.Width = _programsFlowLayoutPanel.Width;
            }
        }

        private static Color MakeColor(EntryDto entry) => entry.Status switch
        {
            Status.Fail => Color.Red,
            Status.Possible => Color.Yellow,
            Status.Pass => Color.Green,
            _ => throw new ArgumentException("Unknown status")
        };
    }
}
