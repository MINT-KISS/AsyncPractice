using AsyncDownloader.Application.Abstractions;
using AsyncDownloader.Application.Services;
using AsyncDownloader.Cli;
using AsyncDownloader.Infrastructure.Services;
using System.Net;

namespace AsyncDownloader.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPostService, PostService>();
            serviceCollection.AddScoped<IHttpDataDownloader, HttpDataDownloader>();
            serviceCollection.AddHttpClient("ExternalApiClient", c =>
                c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"))
                             .ConfigurePrimaryHttpMessageHandler(() =>
                new SocketsHttpHandler { AutomaticDecompression = DecompressionMethods.All });

            return serviceCollection;
        }

        public static IServiceCollection AddAppLogging(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddDebug();
            });

            return servicesCollection;
        }

        public static IServiceCollection AddCli(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ConsoleApp>();
            return serviceCollection;
        }
    }
}
