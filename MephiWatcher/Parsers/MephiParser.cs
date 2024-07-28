using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace MephiWatcher.Parsers
{
    [VuzParserName("mephi")]
    public class MephiParser(IHttpClientProvider httpClientProvider) : IVuzParser
    {
        private readonly HttpClient _httpClient = httpClientProvider.Client;
        private readonly HtmlParser _htmlParser = new();

        public async Task<UniversityProgram[]> ParseProgramsAsync(Uri url, CancellationToken ct = default)
        {
            var html = await GetHtmlAsync(url, ct);
            var parse = _htmlParser.ParseDocument(html);
            var satisfying = parse.GetElementsByTagName("tr").Skip(2);
            var programs = satisfying.Select(e => TrToProgram(e, url));
            return programs.ToArray();
        }

        public async Task<ProgramRating> RarseProgramRatingAsync(UniversityProgram program, CancellationToken ct)
        {
            var programHtml = await GetHtmlAsync(program.Url, ct);
            var parsed = _htmlParser.ParseDocument(programHtml);
            var rows = parsed.GetElementsByTagName("tr").Skip(3).Where(el => el.GetElementsByTagName("th").Length == 0);

            int i = 0;
            var entries = rows.Select(r => TrToEntry(r, program)).OrderByDescending(e => e.Points)
                            .Select(e => e with { SerialNumber = ++i }).ToArray();
            return new ProgramRating(program, entries);
        }

        private static Entry TrToEntry(IElement element, UniversityProgram program)
        {
            var serial = int.Parse(element.Children[0].TextContent.Trim());
            var document = AbiturDocumentFactory.Create(element.Children[1].TextContent.Trim());
            var pointsText = element.Children[4].TextContent;
            int points;
            if (string.IsNullOrEmpty(pointsText) || pointsText == "-")
                points = 310;
            else points = int.Parse(pointsText);
            return new Entry(program, serial, document, points, Status.Pass);
        }

        private static UniversityProgram TrToProgram(IElement element, Uri baseUrl)
        {
            var name = element.Children[0].TextContent;
            var url = element.Children[1].Children[0].GetAttribute("href") ?? throw new ArgumentException("Row does not have a link attached");
            return new UniversityProgram(name.Trim(), new Uri(baseUrl.GetLeftPart(UriPartial.Authority) + url));
        }

        private async Task<string> GetHtmlAsync(Uri url, CancellationToken ct) => await _httpClient.GetStringAsync(url, ct);
    }
}
