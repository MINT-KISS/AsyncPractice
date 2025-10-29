using AsyncDownloader.Application.Abstractions;

namespace AsyncDownloader.Infrastructure.Services
{
    internal class HttpDataDownloader(IHttpClientFactory httpClientFactory,
                                      ILogger<HttpDataDownloader> logger) : IHttpDataDownloader
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExternalApiClient");

        public async Task<string> DownloadDataAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            const int maxAttempts = 3;
            var baseDelay = TimeSpan.FromMilliseconds(200);

            try
            {
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        using var response = await _httpClient.GetAsync(
                            requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new HttpRequestException($"Request failed with status {response.StatusCode}",
                                                           null, response.StatusCode);
                        }

                        return await response.Content.ReadAsStringAsync(cancellationToken);
                    }
                    catch (HttpRequestException ex) when (attempt < maxAttempts)
                    {
                        logger.LogDebug(ex, "Retry {Attempt}/{Max} for {Uri}", attempt, maxAttempts, requestUri);
                        await Task.Delay(TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * attempt), cancellationToken);
                    }
                    catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested && attempt < maxAttempts)
                    {
                        logger.LogDebug(ex, "Retry {Attempt}/{Max} (timeout) for {Uri}", attempt, maxAttempts, requestUri);
                        await Task.Delay(TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * attempt), cancellationToken);
                    }
                }

                throw new HttpRequestException("Request failed after retries.");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Failed to download from {Uri}", requestUri);
                throw;
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Timeout downloading from {Uri}", requestUri);
                throw new TimeoutException("Request timed out.", ex);
            }
        }
    }
}
