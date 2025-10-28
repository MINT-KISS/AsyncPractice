using AsyncDownloader.Domain.Models;

namespace AsyncDownloader.Application.Abstractions
{
    public interface IPostService
    {
        Task <IEnumerable<Post?>> GetPostsAsync(CancellationToken cancellationToken = default);
        Task<Post?> GetPostByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
