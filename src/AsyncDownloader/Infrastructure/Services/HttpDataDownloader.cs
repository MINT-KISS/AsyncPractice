using AsyncDownloader.Application.Abstractions;

namespace AsyncDownloader.Infrastructure.Services
{
    internal class HttpDataDownloader(IHttpClientFactory httpClientFactory) : IHttpDataDownloader
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExternalApiClient");

        public async Task<string> DownloadDataAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
