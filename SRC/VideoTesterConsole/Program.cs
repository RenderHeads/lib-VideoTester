using System;
using System.Collections.Generic;
using System.Linq;
using LibVideoTester;
using LibVideoTester.Factories;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
using LibVideoTester.Api;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Threading.Tasks;
using System.CommandLine;
using System.IO;

namespace VideoTester
{
    class Program
    {
        static Serilog.Core.Logger log;
        static async Task<int> Main(string[] args)
        {
            log = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Code).CreateLogger();
            log.Information("Video Checker (C) RenderHeads 2023");
            //if there aren't enough args just force a help command. Couldn't find a way to make this the default behaviour.
            //TODO: See if we can mmake this default behaviour of RootCommand?
            if (args.Length <= 1)
            {
                args = new string[] { "-?" };
            }

            var fileOption = new Option<string>(
            name: "-i",
            description: "the input stream to check against, for example -i movie.mov");

            var rootCommand = new RootCommand("Video Sanity Testing Tool. It will check a video against some known configurations you have specified in the Configurations folder. ");
            
            rootCommand.AddOption(fileOption);
            rootCommand.SetHandler(async (path) =>
            {
                await CheckVideo(path);
            },
                fileOption);

            return await rootCommand.InvokeAsync(args);

        }

        static async Task CheckVideo(string pathToVideo)
        {
            VideoMetaData v = await VideoTesterApi.GetVideoMetaDataAsync(pathToVideo);
            log.Information("Meta data retrieved from file width:{width}, height:{height}, bitrate:{bitrate}, fps: {fps}, codec: {codec}",
                v.Width,
                v.Height,
                v.BitrateKPBS,
                v.FrameRate,
                v.Codec);

            Dictionary<string, Configuration> configs = await VideoTesterApi.GetConfigurationsAsync();
            if (configs.Keys.Count == 0)
            {
                log.Error("No valid configurations found, exiting");

            }
            else
            {
                log.Information("Found {num} configurations in Configurations folder", configs.Keys.Count);
                foreach (var key in configs.Keys)
                {
                    log.Information("Configuration {key} - {value}", key, configs[key]);
                }

                Dictionary<string, Configuration> matches = VideoTesterApi.FindMatches(v, configs);
                bool foundMatch = matches.Keys.Count > 0;

                if (!foundMatch)
                {
                    log.Error("Unable to find match with a configuration this file is not valid!");
                }
                else
                {
                    log.Information("Video matches atleast {count} configurations", configs.Keys.Count);
                }
            }
        }
    }
}

