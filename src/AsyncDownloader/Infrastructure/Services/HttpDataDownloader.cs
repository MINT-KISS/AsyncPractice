using AsyncDownloader.Application.Abstractions;

namespace AsyncDownloader.Infrastructure.Services
{
    internal class HttpDataDownloader(IHttpClientFactory httpClientFactory,
                                      ILogger<HttpDataDownloader> logger) : IHttpDataDownloader
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExternalApiClient");

        public async Task<string> DownloadDataAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning("HTTP {StatusCode} for {Uri}", response.StatusCode, requestUri);
                    throw new HttpRequestException($"Request failed with status {response.StatusCode}", null, response.StatusCode);
                }

                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "HTTP request failed for {Uri}", requestUri);
                throw;
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Request timed out for {Uri}", requestUri);
                throw new TimeoutException("Request timed out.", ex);
            }
            catch (OperationCanceledException)
            {
                logger.LogDebug("Request canceled for {Uri}", requestUri);
                throw;
            }
        }
    }
}
