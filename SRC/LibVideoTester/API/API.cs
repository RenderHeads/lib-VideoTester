using System;
using System.Threading.Tasks;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
using LibVideoTester.Providers;
using LibVideoTester.Factories;

namespace LibVideoTester.Api
{
    public static class VideoTesterApi
    {
        public static async Task<VideoMetaData> GetVideoInfo(string path, IVideoMetaDataProvider metaDataProvider = null, IDeserializer<VideoMetaData> deserializer = null)
        {
            IVideoMetaDataProvider providerToUse = metaDataProvider == null ? new FFProbeMetaDataProvider() : metaDataProvider;
            IDeserializer<VideoMetaData> deserilizerToUse = deserializer == null ? new FFprobeMetaToVideoInfo() : deserializer;
            VideoMetaDataFactory videoInfoGenerator = new VideoMetaDataFactory(providerToUse, deserilizerToUse);
            return await videoInfoGenerator.GetVideoInfoAsync(path);
        }
    }
}

