﻿// Program.cs 2020

namespace avcli
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        internal static int Main(string[] args)
        {
            // Create a root command with some options
            var rootCommand = new RootCommand
    {
        new Option<int>(
            "--int-option",
            getDefaultValue: () => 42,
            description: "An option whose argument is parsed as an int"),
        new Option<bool>(
            "--bool-option",
            "An option whose argument is parsed as a bool"),
        new Option<FileInfo>(
            "--file-option",
            "An option whose argument is parsed as a FileInfo")
    };

            rootCommand.Description = "My sample app";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<int, bool, FileInfo>((intOption, boolOption, fileOption) =>
             {
                 Console.WriteLine($"The value for --int-option is: {intOption}");
                 Console.WriteLine($"The value for --bool-option is: {boolOption}");
                 Console.WriteLine($"The value for --file-option is: {fileOption?.FullName ?? "null"}");
             });

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }
    }
}