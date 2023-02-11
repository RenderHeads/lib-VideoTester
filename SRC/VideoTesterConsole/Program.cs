using System;
using System.Collections.Generic;
using System.Linq;
using LibVideoTester;
using LibVideoTester.Factories;
using LibVideoTester.Helpers;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
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
            VideoMetaDataFactory videoInfoGenerator = new VideoMetaDataFactory(new FFProbeMetaDataProvider(), new FFprobeMetaToVideoInfo());
            if (args.Length >= 1)
            {  
                log.Information("Checking Video {path} ", args[0]);
                VideoMetaData v = videoInfoGenerator.GetVideoInfoAsync(args[0]).GetAwaiter().GetResult();
                log.Information("Meta data retrieved from file width:{width}, height:{height}, bitrate:{bitrate}, fps: {fps}, codec: {codec}",
                    v.Width,
                    v.Height,
                    v.BitrateKPBS,
                    v.FrameRate,
                    v.Codec);

                ConfigurationFactory configurationReader = new ConfigurationFactory(new JsonFileProvider(),
                    new NewtonSoftJsonDeserializer<Configuration>());
                List<Configuration> configurations = GenerateStandardConfig();
                configurationReader.ReadConfigurations("Configurations");
                if (configurationReader.GetConfigurationCount() > 0)
                {
                    Dictionary<string, Configuration> configs = configurationReader.GetConfigurations().GetAwaiter().GetResult();
                    configurations = configs.Values.ToList();
                    log.Information("Found {num} configurations in Configurations folder", configurations);
                    foreach (var key in configs.Keys)
                    {
                        log.Information("Configuration {key} - {value}", key, configs[key]);
                    }
                }
                bool foundMatch = ConfigurationMatcher.TryGetMatches(v, out configurations, configurations);
                if (!foundMatch)
                {
                    log.Error("Unable to find match with a configuration this file is not valid!");
                }
                else
                {
                    log.Information("Video matches atleast {count} configurations", configurations.Count);
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
            return new List<Configuration> { new Configuration(new string[] { "h264"}, 2048,2048,new int[] { 30,60}, 30000)};
        }
    }
}

