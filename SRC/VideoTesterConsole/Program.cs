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

namespace VideoTester
{
    class Program
    {
        static void Main(string[] args)
        {
            using var log = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Code).CreateLogger();
            log.Information("Video Checker (C) RenderHeads 2023");
            //NOTE: We assume ffprobe is installed and available on your path ENV.        
            if (args.Length >= 1)
            {
                log.Information("Checking Video {path} ", args[0]);
                VideoMetaData v = VideoTesterApi.GetVideoMetaDataAsync(args[0]).GetAwaiter().GetResult();
                log.Information("Meta data retrieved from file width:{width}, height:{height}, bitrate:{bitrate}, fps: {fps}, codec: {codec}",
                    v.Width,
                    v.Height,
                    v.BitrateKPBS,
                    v.FrameRate,
                    v.Codec);

                Dictionary<string, Configuration> configs = VideoTesterApi.GetConfigurationsAsync().GetAwaiter().GetResult();
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
            else
            {
                log.Error("you need to pass in an argument to a file to test it");
            }
            log.Information("ALL DONE! Press any key to continue");
            Console.ReadLine();
        }

        static List<Configuration> GenerateStandardConfig()
        {
            return new List<Configuration> { new Configuration(new string[] { "h264" }, 2048, 2048, new int[] { 30, 60 }, 30000) };
        }
    }
}

