using AsyncDownloader.Application.Abstractions;
using AsyncDownloader.Application.Services;
using AsyncDownloader.Infrastructure.Services;

namespace AsyncDownloader.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPostService, PostService>();
            serviceCollection.AddScoped<IHttpDataDownloader, HttpDataDownloader>();
            return serviceCollection;
        }
    }
}
