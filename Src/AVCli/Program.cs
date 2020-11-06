// Program.cs 2020

namespace AVCli
{
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Hosting;
    using System.CommandLine.Invocation;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.CommandLine.Parsing;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using AVCli.AVLib;
    using AVCli.AVLib.Extractor;
    using AVCli.AVLib.Services;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net;
    using System.Linq;

    class Program
    {
        static async Task Main(string[] args) => await BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    Configuration configuration = new Configuration();
                    host.ConfigureServices(services =>
                    {
                        services.AddSingleton(configuration);
                        services.AddSingleton<IExtractor,JavDBExtractor>();
                        services.AddSingleton<ICacheProvider,LiteDBCacheProvider>();
                        var proxySelector = new RoundRobinProxySelector(configuration);
                        services.AddSingleton<IProxySelector>(proxySelector);
                        if (!configuration.UseProxy)
                        {
                            services.AddHttpClient(Configuration.NoProxy);
                        }
                        else
                        {
                            var all = proxySelector.GetAll();
                            foreach(var p in all)
                            {
                                services.AddHttpClient(p.Key).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                                {
                                    Proxy = new WebProxy(p.Value)
                                });
                            }
                        }
                        services.AddSingleton<IHtmlContentReader, DefaultHtmlContentReader>();
                    });
                })
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var root = new RootCommand(@"$ dotnet run --number 'MUM_120'"){
                new Option<string>("--number"){
                    IsRequired = true
                }
            };
            root.Handler = CommandHandler.Create<NumberOptions, IHost>(Run);
            return new CommandLineBuilder(root);
        }

        private static async Task Run(NumberOptions options, IHost host)
        {
            var serviceProvider = host.Services;
            var extractor = serviceProvider.GetService<IExtractor>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));

            var data = await extractor.GetDataAsync(options.Number);
            logger.LogInformation(data.Number);
            
        }
    }
}