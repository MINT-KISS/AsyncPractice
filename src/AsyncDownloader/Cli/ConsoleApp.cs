using AsyncDownloader.Application.Abstractions;
using AsyncDownloader.Domain.Models;

namespace AsyncDownloader.Cli
{
    public class ConsoleApp(IPostService postService)
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("\nEnter post id (1-99 or 'all') or 'exit':");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input == "exit") break;
                
                if (input == "all")
                {
                    Console.WriteLine();
                    var posts = await postService.GetPostsAsync(cancellationToken);
                    Console.WriteLine();
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
                Console.WriteLine();
                if (post is null)
                {
                    Console.WriteLine("Not found.");
                    continue;
                }

                PrintPost(post);
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
