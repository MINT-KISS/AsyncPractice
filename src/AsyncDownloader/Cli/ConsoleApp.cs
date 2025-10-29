using AsyncDownloader.Application.Abstractions;
using AsyncDownloader.Domain.Models;

namespace AsyncDownloader.Cli
{
    public class ConsoleApp(IPostService postService, ILogger<ConsoleApp> logger)
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("\nEnter post id (1-99 or 'all') or 'exit':");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input == "exit") break;

                try
                {
                    if (input == "all")
                    {
                        Console.WriteLine();
                        var posts = await postService.GetPostsAsync(cancellationToken);
                        foreach (var p in posts)
                        {
                            if (p is not null) PrintPost(p);
                        }
                        continue;
                    }

                    if (!int.TryParse(input, out var id) || id < 1 || id > 99)
                    {
                        Console.WriteLine("Invalid id.");
                        continue;
                    }

                    Console.WriteLine();
                    var post = await postService.GetPostByIdAsync(id, cancellationToken);
                    if (post is null)
                    {
                        Console.WriteLine("Not found.");
                        continue;
                    }

                    PrintPost(post);
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Request timed out. Please try again.");
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Network error. Check your connection and try again.");
                }
                catch (Exception)
                {
                    Console.WriteLine("An error occurred. Details are logged.");
                }
            }
        }

        private void PrintPost(Post post)
        {
            Console.WriteLine($"""
            ---
            Id:     {post.Id}
            UserId: {post.UserId}
            Title:  {post.Title}
            Body:   {post.Body}
            ---
            """);
        }
    }
}
