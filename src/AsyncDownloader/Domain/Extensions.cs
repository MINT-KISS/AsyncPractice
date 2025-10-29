using AsyncDownloader.Application.Abstractions;
using AsyncDownloader.Application.Services;
using AsyncDownloader.Cli;
using AsyncDownloader.Infrastructure.Services;

namespace AsyncDownloader.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPostService, PostService>();
            serviceCollection.AddScoped<IHttpDataDownloader, HttpDataDownloader>();
            serviceCollection.AddHttpClient("ExternalApiClient", c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

            return serviceCollection;
        }

        public static IServiceCollection AddAppLogging(this IServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddDebug();
            });

            return services;
        }
        
        public static IServiceCollection AddCli(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ConsoleApp>();
            return serviceCollection;
        }
    }
}
