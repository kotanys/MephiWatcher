namespace MephiWatcher
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly HttpClient _httpClient = new();
        public HttpClient Client => _httpClient;
    }

    public interface IHttpClientProvider
    {
        HttpClient Client { get; }
    }
}
