using AsyncDownloader.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AsyncDownloader.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController(IPostService postService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get(CancellationToken cancellationToken = default)
        {
            var posts = await postService.GetPostsAsync(cancellationToken);
            return posts != null && posts.Any() ? Ok(posts) : NotFound();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var post = await postService.GetPostByIdAsync(id, cancellationToken);
            return post != null ? Ok(post) : NotFound();
        }
    }
}
