// Program.cs 2020

namespace AVCli
{
    using AVCli.AVLib;
    using AVCli.AVLib.Extensions;
    using AVCli.AVLib.Extractor;
    using AVCli.AVLib.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Hosting;
    using System.CommandLine.Invocation;
    using System.CommandLine.Parsing;
    using System.CommandLine.Rendering;
    using System.CommandLine.Rendering.Views;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        internal static async Task Main(string[] args) => await BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    host.ConfigureServices(services =>
                    {
                        var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json").Build();

                        services.Configure<Configuration>(config.GetSection("AVCli"));
                        Configuration configuration = new Configuration();
                        config.Bind("AVCli", configuration);
                        services.AddSingleton<IExtractor, JavDBExtractor>();
                        services.AddSingleton<ICacheProvider, LiteDBCacheProvider>();
                        services.AddSingleton(configuration);
                        var proxySelector = new RoundRobinProxySelector(configuration);
                        services.AddSingleton<IProxySelector>(proxySelector);
                        if (!configuration.UseProxy)
                        {
                            services.AddHttpClient(Configuration.NoProxy);
                        }
                        else
                        {
                            var all = proxySelector.GetAll();
                            foreach (var p in all)
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

        /// <summary>
        /// The BuildCommandLine.
        /// </summary>
        /// <returns>The <see cref="CommandLineBuilder"/>.</returns>
        private static CommandLineBuilder BuildCommandLine()
        {
            var root = new RootCommand(@"$ dotnet run 'MUM_120'") { };
            root.AddArgument(new Argument<string>("number", "number of av"));
            root.Handler = CommandHandler.Create<string, List<string>, IHost>(Run);
            return new CommandLineBuilder(root);
        }

        /// <summary>
        /// The Run.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <param name="proxies">The proxies<see cref="List{string}"/>.</param>
        /// <param name="host">The host<see cref="IHost"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task Run(string number, List<string> proxies, IHost host)
        {
            var region = new Region(0, 0, Console.WindowWidth, Console.WindowHeight, false);
            var serviceProvider = host.Services;
            var context = serviceProvider.GetService<InvocationContext>();
            var console = context.Console;
            if (console is ITerminal terminal)
            {
                terminal.Clear();
            }

            var consoleRenderer = new ConsoleRenderer(
                console,
                mode: context.BindingContext.OutputMode(),
                resetAfterRender: true);
            var extractor = serviceProvider.GetService<IExtractor>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));

            var data = await extractor.GetDataAsync(number);
            logger.LogDebug(data.Number);
            var table = new TableView<AvData>
            {
                Items = new List<AvData> { data }
            };
            table.AddColumn(data => $"{data.Title.Shorten(8)}", "Title");
            table.AddColumn(data => $"{data.Number}", "Number");
            table.AddColumn(data => $"{data.Time}", "Time");
            table.AddColumn(data => $"{data.Actors?.Join(",")}", "Actors");
            table.AddColumn(data => $"{data.Tags?.Join(",")}", "Tags");
            //table.AddColumn(process => ContentView.FromObservable(process.TrackCpuUsage(), x => $"{x.UsageTotal:P}"), "CPU", ColumnDefinition.Star(1));

            var screen = new ScreenView(renderer: consoleRenderer, console) { Child = table };
            screen.Render(region);
        }
    }
}
