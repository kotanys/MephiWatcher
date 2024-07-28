using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CefSharp;
using CefSharp.OffScreen;

namespace MephiWatcher.Parsers
{
    [VuzParserName("mirea")]
    public class MireaParser: IVuzParser
    {
        private readonly HtmlParser _htmlParser = new();

        public async Task<UniversityProgram[]> ParseProgramsAsync(Uri url, CancellationToken ct)
        {
            var html = await GetHtmlOfLoadedPage(url, ct);
            var parse = await _htmlParser.ParseDocumentAsync(html);
            var results = ParseTable(parse).Select(zip => ZipToProgram(zip.Header, zip.Link, url));
            return results.ToArray();
        }

        private static IEnumerable<(IElement Header, IElement Link)> ParseTable(IHtmlDocument parse)
        {
            var table = parse.QuerySelector("tbody")!;
            var tableRows = table.Children.OfType<IHtmlTableRowElement>().ToArray();
            List<IElement> headers = [], linkRows = [];
            for (int i = 0; i < tableRows.Length; i++)
            {
                var element = tableRows[i];
                if (element.ClassList.Contains("css-bgi5uq"))
                {
                    headers.Add(element);
                    linkRows.Add(tableRows[++i]);
                }
            }
            var zipped = headers.Zip(linkRows);
            return zipped;
        }

        private static UniversityProgram ZipToProgram(IElement headerRow, IElement linkRow, Uri baseUrl)
        {
            var name = headerRow.Children[0].Children[0].InnerHtml.Trim();
            var url = linkRow.Children[5].Children[0].GetAttribute("href");
            return new UniversityProgram(name, new Uri(baseUrl.GetLeftPart(UriPartial.Authority) + url));
        }

        public async Task<ProgramRating> RarseProgramRatingAsync(UniversityProgram program, CancellationToken ct)
        {
            var html = await GetHtmlOfLoadedPage(program.Url, ct);
            var parse = await _htmlParser.ParseDocumentAsync(html);
            var rows = parse.GetElementsByTagName("tbody").Single().GetElementsByTagName("tr")
                .Where(tr => tr.Children[4].InnerHtml == "да");

            int i = 1;
            var entries = rows.Select(tr => TrToEntry(tr, program, i++));
            return new ProgramRating(program, entries.ToArray());
        }

        private static Entry TrToEntry(IElement entry, UniversityProgram program, int index)
        {
            var document = AbiturDocumentFactory.Create(entry.GetAttribute("data-snils")!);
            var points = int.Parse(entry.Children[10].InnerHtml);
            return new Entry(program, index, document, points, Status.Pass);
        }


        /// <summary>
        /// <para>Загружает страницу по адресу <paramref name="url"/>, выполняет все скрипты на ней и возвращает её html.</para>
        /// <para>Я ненавижу реализацию этого метода каждой частичкой своей души.</para>
        /// </summary>
        /// <param name="url">Url страницы, которую нужно загрузить.</param>
        /// <param name="ct"></param>
        /// <returns>HTML загруженной страницы.</returns>
        private async Task<string> GetHtmlOfLoadedPage(Uri url, CancellationToken ct)
        {
            string html;
            IElement? table = null;

            const int UpdateWaitMs = 400;
            const int TimeoutMs = UpdateWaitMs * 3;

            using (var browser = new ChromiumWebBrowser(url.ToString()))
            {

                var tcs = new TaskCompletionSource<bool>();
                browser.LoadingStateChanged += (sender, args) =>
                {
                    if (ct.IsCancellationRequested)
                    {
                        tcs.SetCanceled();
                    }
                    if (!args.IsLoading)
                    {
                        tcs.TrySetResult(true);
                    }
                };
                await tcs.Task;

                var timeoutTask = Task.Delay(TimeoutMs, ct);
                do
                {
                    await Task.Delay(UpdateWaitMs, ct);
                    ct.ThrowIfCancellationRequested();
                    html = await browser.GetSourceAsync();
                    var parse = await _htmlParser.ParseDocumentAsync(html);
                    table = parse.QuerySelector("tbody");
                } while (table is null && !timeoutTask.IsCompleted);
            }
            if (table is null)
            {
                html = await GetHtmlOfLoadedPage(url, ct);
            }
            return html;
        }
    }
}
