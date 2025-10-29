using AsyncDownloader.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AsyncDownloader.Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController(IPostService postService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get(CancellationToken cancellationToken = default)
        {
            try
            {
                var posts = await postService.GetPostsAsync(cancellationToken);
                return posts != null && posts.Any() ? Ok(posts) : NotFound();
            }
            catch (TimeoutException)
            {
                return StatusCode(504, "Request to external API timed out.");
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "External API is unavailable.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var post = await postService.GetPostByIdAsync(id, cancellationToken);
                return post != null ? Ok(post) : NotFound();
            }
            catch (TimeoutException)
            {
                return StatusCode(504, "Request to external API timed out.");
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "External API is unavailable.");
            }
        }
    }
}
