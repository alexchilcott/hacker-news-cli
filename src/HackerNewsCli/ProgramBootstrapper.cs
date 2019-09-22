using System.Net.Http;
using HackerNewsCli.CommandLineArguments;
using HackerNewsCli.Console;
using HackerNewsCli.HackerNews;
using HackerNewsCli.HackerNews.Scraping;
using HackerNewsCli.Json;
using HackerNewsCli.Scraping;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNewsCli
{
    public static class ProgramBootstrapper
    {
        private static int Main(string[] args)
        {
            // This method's only responsibility is to register the components of the application
            // in the dependency container, and to then resolve and run the program.

            var services = GetServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            var program = serviceProvider.GetService<Program>();
            return program.RunAsync(args).Result;
        }

        private static IServiceCollection GetServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<Program>();
            services.AddSingleton<IOutputWriter, StandardOutputWriter>();
            services.AddSingleton<IOptionsProvider, OptionsProvider>();
            services.AddSingleton<HttpMessageHandler, HttpClientHandler>();
            services.AddSingleton<IHackerNewsTopPostRetriever, HackerNewsTopPostScraper>();
            services.AddSingleton<IScraper<Post[]>, ConvertingScraper<ScrapedPostContent[], Post[]>>();
            services.AddSingleton<IScraper<ScrapedPostContent[]>, HackerNewsPostPageScraper>();
            services.AddSingleton<IConverter<ScrapedPostContent, Post>, ScrapedPostContentParser>();
            services.AddSingleton<IConverter<ScrapedPostContent[], Post[]>, ArrayConverter<ScrapedPostContent, Post>>();
            services.AddSingleton<IJsonSerializer, PrettyPrintedJsonSerializer>();
            return services;
        }
    }
}