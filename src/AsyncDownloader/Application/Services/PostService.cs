using AsyncDownloader.Application.Abstractions;
using AsyncDownloader.Domain.Models;
using System.Text.Json;

namespace AsyncDownloader.Application.Services
{
    internal class PostService(IHttpDataDownloader downloader) : IPostService
    {
        public async Task<IEnumerable<Post?>> GetPostsAsync(CancellationToken cancellationToken = default)
        {
            var json = await downloader.DownloadDataAsync("posts", cancellationToken);
            return JsonSerializer.Deserialize<IEnumerable<Post?>>(json) ?? [];
        }

        public async Task<Post?> GetPostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var json = await downloader.DownloadDataAsync($"posts/{id}", cancellationToken);
            return JsonSerializer.Deserialize<Post?>(json);
        }
    }
}