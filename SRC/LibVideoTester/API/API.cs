using System;
using System.Threading.Tasks;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
using LibVideoTester.Factories;
using System.Collections.Generic;

namespace LibVideoTester.Api
{
    public static class VideoTesterApi
    {
        public static async Task<VideoMetaData> GetVideoMetaDataAsync(string path, IVideoMetaDataProvider metaDataProvider = null, IDeserializer<VideoMetaData> deserializer = null)
        {
            IVideoMetaDataProvider providerToUse = metaDataProvider == null ? new FFProbeMetaDataProvider() : metaDataProvider;
            IDeserializer<VideoMetaData> deserilizerToUse = deserializer == null ? new FFprobeMetaToVideoInfo() : deserializer;
            VideoMetaDataFactory videoInfoGenerator = new VideoMetaDataFactory(providerToUse, deserilizerToUse);
            return await videoInfoGenerator.GetVideoInfoAsync(path);
        }

        public static async Task<Dictionary<string, Configuration>> GetConfigurationsAsync(string directoryToCheck = "Configurations", IFileProvider fileProvider = null, IDeserializer<Configuration> deserializer = null)
        {
            IFileProvider fileProviderToUse = fileProvider == null ? new JsonFileProvider() : fileProvider;
            IDeserializer<Configuration> deserializerToUser = deserializer == null ? new NewtonSoftJsonDeserializer<Configuration>() : deserializer;
            ConfigurationFactory configurationFactory = new ConfigurationFactory(fileProviderToUse, deserializerToUser);
            configurationFactory.ReadConfigurations(directoryToCheck);
            return await configurationFactory.GetConfigurations();
        }

        public static Dictionary<string, Configuration> FindMatches(VideoMetaData data, Dictionary<string, Configuration> configurations)
        {
            Dictionary<string, Configuration> matches = new Dictionary<string, Configuration>();
            foreach (var kvp in configurations)
            {
                if (data.TestConfiguration(kvp.Value))
                {
                    matches.Add(kvp.Key,kvp.Value);
                }
            }     
            return matches;
        }
    }
}

