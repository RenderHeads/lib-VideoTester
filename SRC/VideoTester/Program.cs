using System;
using System.Collections.Generic;
using LibVideoTester;
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
            VideoInfoGenerator videoInfoGenerator = new VideoInfoGenerator(new FFProbeMetaDataGenerator());
   
            if (args.Length >= 1)
            {
                log.Information("Checking Video {path} ", args[0]);
                VideoInfo v = videoInfoGenerator.GetVideoInfoAsync(args[0]).GetAwaiter().GetResult();
                log.Information("Meta data retrieved from file width:{width}, height:{height}, bitrate:{bitrate}, fps: {fps}, codec: {codec}",
                    v.GetWidth(),
                    v.GetHeight(),
                    v.GetBitrate(),
                    v.GetFramerate(),
                    v.GetCodec());

                //TODO: GetConfig from Json but for now just use something hardcoded:
                List<Configuration> configurations =  new List<Configuration>();
                bool foundMatch = ConfigurationMatcher.TryGetMatches(v, out configurations, GenerateStandardConfig());

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

