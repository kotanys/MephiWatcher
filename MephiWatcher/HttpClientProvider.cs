namespace MephiWatcher
{
    /// <summary>
    /// Class that provides one <see cref="HttpClient"/> for all its users.
    /// </summary>
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly HttpClient _httpClient = new();
        public HttpClient Client => _httpClient;
    }

    /// <summary>
    /// Interface for a type that provides some <see cref="HttpClient"/>.
    /// </summary>
    public interface IHttpClientProvider
    {
        HttpClient Client { get; }
    }
}
