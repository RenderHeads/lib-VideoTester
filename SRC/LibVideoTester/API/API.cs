using System;
using System.Threading.Tasks;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
using LibVideoTester.Factories;
using System.Collections.Generic;
using System.IO;

namespace LibVideoTester.Api {
  public static class VideoTesterApi {
    /// <summary>
    /// Reads video meta data from a video file, such as width, height, bitrate, framerate and codec
    /// </summary>
    /// <param name="path">The path to the file to read</param>
    /// <param name="metaDataProvider">by Default, it will use the FFProbeMetaDataProvider, but you
    /// can create your own for custom behaviour.</param> <param name="deserializer"><by default,
    /// will use the FFProbeMetaDataToVideoInfo class, but you can create your own for custom
    /// behaviour (for example if you mutate the incoming data somehow from standard ffprobe
    /// output)./param> <returns>A VideoInfo object populated with the information it was able to
    /// retrieve. Don't assume that if you get an object, that the readings are valid
    /// though.</returns>
    public static async Task<VideoMetaData> GetVideoMetaDataAsync(
        string path,
        IVideoMetaDataProvider metaDataProvider = null,
        IDeserializer<VideoMetaData> deserializer = null) {
      IVideoMetaDataProvider providerToUse =
          metaDataProvider == null ? new FFProbeMetaDataProvider() : metaDataProvider;
      IDeserializer<VideoMetaData> deserilizerToUse =
          deserializer == null ? new FFprobeMetaToVideoInfo() : deserializer;
      VideoMetaDataFactory videoInfoGenerator =
          new VideoMetaDataFactory(providerToUse, deserilizerToUse);
      return await videoInfoGenerator.GetVideoInfoAsync(path);
    }

    /// <summary>
    /// Get all configuration files for in a specified directory, by default it will use the
    /// "Configurations" directory.
    /// </summary>
    /// <param name="directoryToCheck">The path to check, by default it will only look for .json
    /// files.</param> <param name="fileProvider">By default will use JsonFileProvider, but you can
    /// create your own if you want to serve files from non disk locations.</param> <param
    /// name="deserializer">By default will use NewtonSoftJsonDeserializer, but you can create your
    /// own for custom behaviours.</param> <returns>A dictionary of configurations, where the key is
    /// the path to the file containing the config, and the value is the Configuration object
    /// generated from it.</returns>
    public static async Task<Dictionary<string, Configuration>> GetConfigurationsAsync(
        string directoryToCheck = "Configurations",
        IFileProvider fileProvider = null,
        IDeserializer<Configuration> deserializer = null) {
      IFileProvider fileProviderToUse =
          fileProvider == null ? new JsonFileProvider() : fileProvider;
      IDeserializer<Configuration> deserializerToUser =
          deserializer == null ? new NewtonSoftJsonDeserializer<Configuration>() : deserializer;
      ConfigurationFactory configurationFactory =
          new ConfigurationFactory(fileProviderToUse, deserializerToUser);
      configurationFactory.ReadConfigurations(directoryToCheck);
      return await configurationFactory.GetConfigurations();
    }

    /// <summary>
    /// Use this to compare your metadata against configurations.
    /// </summary>
    /// <param name="data">The video metadata to check</param>
    /// <param name="configurations">The configurations to check against</param>
    /// <returns>A dictionary of all configurations that matched the video</returns>
    public static Dictionary<string, Configuration> FindMatches(
        VideoMetaData data,
        Dictionary<string, Configuration> configurations) {
      Dictionary<string, Configuration> matches = new Dictionary<string, Configuration>();
      foreach (var kvp in configurations) {
        if (data.TestConfiguration(kvp.Value)) {
          matches.Add(kvp.Key, kvp.Value);
        }
      }
      return matches;
    }

    /// <summary>
    /// A easy to use helper method to that works as a oneline to get all matching configurations
    /// for a given video
    /// </summary>
    /// <param name="pathToVideo">The path to the video to check</param>
    /// <param name="metaDataProvider">by Default, it will use the FFProbeMetaDataProvider, but you
    /// can create your own for custom behaviour.</param> <param name="fileProvider">By default will
    /// use JsonFileProvider, but you can create your own if you want to serve files from non disk
    /// locations.</param> <returns>A dictionary of all configurations that matched the
    /// video</returns>
    public static async Task<Dictionary<string, Configuration>>
    ExtractMetaDataAndFindConfigMatchesAsync(string pathToVideo,
                                             IVideoMetaDataProvider metaDataProvider = null,
                                             IFileProvider fileProvider = null) {
      IVideoMetaDataProvider metaDataProviderToUse =
          metaDataProvider == null ? new FFProbeMetaDataProvider() : metaDataProvider;
      IFileProvider fileProviderToUse =
          fileProvider == null ? new JsonFileProvider() : fileProvider;

      VideoMetaData metaData = await GetVideoMetaDataAsync(pathToVideo, metaDataProviderToUse);
      Dictionary<string, Configuration> configs =
          await GetConfigurationsAsync("Configurations", fileProviderToUse);
      return FindMatches(metaData, configs);
    }
  }
}
