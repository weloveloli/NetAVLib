﻿// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVCli
{
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
    using System.Threading.Tasks;
    using ConsoleTables;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Weloveloli.AVLib;
    using Weloveloli.AVLib.Extensions;
    using Weloveloli.AVLib.Extractor;
    using Weloveloli.AVLib.Services;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args.</param>
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
            root.AddOption(new Option<bool>(new string[] { "--table", "-t" }, () => false, "set table view"));
            root.AddOption(new Option<bool>(new string[] { "--search", "-s" }, () => false, "enable search"));
            root.Handler = CommandHandler.Create<string, List<string>, bool, bool, IHost>(Run);
            return new CommandLineBuilder(root);
        }

        /// <summary>
        /// The Run.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <param name="proxies">The proxies<see cref="List{String}"/>.</param>
        /// <param name="table">The table<see cref="bool"/>.</param>
        /// <param name="search">the search<see cref="bool"/>.</param>
        /// <param name="host">The host<see cref="IHost"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task Run(string number, List<string> proxies, bool table, bool search, IHost host)
        {

            var serviceProvider = host.Services;

            var extractor = serviceProvider.GetService<IExtractor>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));
            List<AvData> list;
            if (!search)
            {
                var data = await extractor.GetDataAsync(number);
                list = new List<AvData> { data };
                logger.LogDebug(data.Number);
            }
            else
            {
                list = await extractor.SearchDataAsync(number);
            }

            RenderData(list, table, serviceProvider);
        }

        /// <summary>
        /// The RenderData.
        /// </summary>
        /// <param name="avDatas">The avDatas<see cref="List{AvData}"/>.</param>
        /// <param name="table">The table<see cref="bool"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public static void RenderData(List<AvData> avDatas, bool table, IServiceProvider serviceProvider)
        {
            if (table)
            {
                RenderTableData(avDatas, serviceProvider);
            }
            else
            {
                RenderConsoleData(avDatas, serviceProvider);
            }
        }

        /// <summary>
        /// The RenderTableData.
        /// </summary>
        /// <param name="avDatas">The avDatas<see cref="List{AvData}"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public static void RenderTableData(List<AvData> avDatas, IServiceProvider serviceProvider)
        {

            var region = new Region(0, 0, Console.WindowWidth, Console.WindowHeight, false);
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
            var tableView = new TableView<AvData>
            {
                Items = avDatas
            };
            tableView.AddColumn(data => $"{data.Title.Shorten(8)}", "Title");
            tableView.AddColumn(data => $"{data.Number}", "Number");
            tableView.AddColumn(data => $"{data.Time}", "Time");
            tableView.AddColumn(data => $"{data.Actors?.Join(",")}", "Actors");
            tableView.AddColumn(data => $"{data.Tags?.Join(",")}", "Tags");
            //table.AddColumn(process => ContentView.FromObservable(process.TrackCpuUsage(), x => $"{x.UsageTotal:P}"), "CPU", ColumnDefinition.Star(1));

            var screen = new ScreenView(renderer: consoleRenderer, console) { Child = tableView };
            screen.Render(region);
        }

        /// <summary>
        /// The RenderConsoleData.
        /// </summary>
        /// <param name="avDatas">The avDatas<see cref="List{AvData}"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public static void RenderConsoleData(List<AvData> avDatas, IServiceProvider serviceProvider)
        {
            avDatas.ForEach(e =>
            {
                var values = new List<NameValue>
               {
                   NameValue.Of("Number",e.Number),
                   NameValue.Of("Title",e.Title),
                   NameValue.Of("Year",e.Year),
                   NameValue.Of("Time",e.Time),
                   NameValue.Of("MainCover",e.MainCover),
                   NameValue.Of("ThumbUrl",e.ThumbUrl),
                   NameValue.Of("WebSiteUrl",e.WebSiteUrl),
                   NameValue.Of("PreviewVideo",e.PreviewVideo),
                   NameValue.Of("Outline",e.Outline),
                   NameValue.Of("Source",e.Source),
                   NameValue.Of("Studio",e.Studio),
                   NameValue.Of("Actors",e.Actors),
                   NameValue.Of("Director",e.Directors),
                   NameValue.Of("Tags",e.Tags),

               };
                values.AddRange(NameValue.AsList("Covers", e.Covers));
                values.AddRange(NameValue.AsList("Magnets", e.Magnets));

                ConsoleTable.From(values).Configure(o =>
                {
                    o.NumberAlignment = Alignment.Right;
                    o.EnableCount = false;
                }
                ).Write();
                Console.WriteLine();
            });
        }

        /// <summary>
        /// Defines the <see cref="NameValue" />.
        /// </summary>
        internal class NameValue
        {
            /// <summary>
            /// Gets the Name.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the Value.
            /// </summary>
            public string Value { get; private set; }

            /// <summary>
            /// The Of.
            /// </summary>
            /// <param name="name">The name<see cref="string"/>.</param>
            /// <param name="value">The value<see cref="string"/>.</param>
            /// <returns>The <see cref="NameValue"/>.</returns>
            public static NameValue Of(string name, string value)
            {
                return new NameValue { Name = name, Value = value };
            }

            /// <summary>
            /// The Of.
            /// </summary>
            /// <param name="name">The name<see cref="string"/>.</param>
            /// <param name="value">The value<see cref="List{string}"/>.</param>
            /// <returns>The <see cref="NameValue"/>.</returns>
            public static NameValue Of(string name, List<string> value)
            {
                return new NameValue { Name = name, Value = value?.Join(",") };
            }

            /// <summary>
            /// The AsList.
            /// </summary>
            /// <param name="name">The name<see cref="string"/>.</param>
            /// <param name="value">The value<see cref="List{string}"/>.</param>
            /// <returns>The <see cref="List{NameValue}"/>.</returns>
            public static List<NameValue> AsList(string name, List<string> value)
            {
                return value?.Select((e, i) =>
                {
                    return new NameValue { Name = name + i, Value = e };
                }).ToList() ?? new List<NameValue>();
            }
        }
    }
}
