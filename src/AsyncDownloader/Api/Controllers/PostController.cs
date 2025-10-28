using AsyncDownloader.Application.Abstractions;
using AsyncDownloader.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace AsyncDownloader.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController(IPostService postService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Post?>> Get(CancellationToken cancellationToken = default) =>
            await postService.GetPostsAsync(cancellationToken);

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id, CancellationToken cancellationToken)
        {
            var post = await postService.GetPostByIdAsync(id, cancellationToken);
            return post != null ? Ok(post) : NotFound();
        }
    }
}
