namespace AsyncDownloader.Application.Abstractions
{
    public interface IHttpDataDownloader
    {
        Task<string> DownloadDataAsync(string requestUri, CancellationToken cancellationToken);
    }
}
