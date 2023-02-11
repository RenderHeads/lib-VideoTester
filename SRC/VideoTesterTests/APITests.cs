using System;
using LibVideoTester.Factories;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
using NUnit.Framework;
using System.Threading.Tasks;
using LibVideoTester.Api;
using System.Collections.Generic;
using System.Linq;

namespace VideoTesterTests
{
    public class APITests
    {
        //A way to generate some mock data for our application so that we get valid json for the application to test against
        public class MockFileProvider : IFileProvider
        {
            private Dictionary<string, string> VideoInfoAsJsonKVP = new Dictionary<string, string>();

            public MockFileProvider(Dictionary<string, Configuration> mockFiles)
            {
                foreach (var key in mockFiles.Keys)
                {
                    VideoInfoAsJsonKVP[key] = Newtonsoft.Json.JsonConvert.SerializeObject(mockFiles[key]);
                }
            }

            public async Task<string> GetFileContentsAsync(string pathToFile)
            {
                if (VideoInfoAsJsonKVP.ContainsKey(pathToFile))
                {
                    return VideoInfoAsJsonKVP[pathToFile];
                }
                return "";
            }

            public string[] GetFullPathToFilesInDirectory(string directoryPath)
            {
                return VideoInfoAsJsonKVP.Keys.ToArray();
            }
        }

        private Dictionary<string, Configuration> DummyData = new Dictionary<string, Configuration>()
            {
            {"/path1", new Configuration("Config 1", new string[]{"hap","h264"}, 4096,4096,new int[]{30,60,25},999999)},
            {"/path2", new Configuration("Config 2", new string[]{"hevc","h264"}, 512,768,new int[]{25},1 )}

            };
        private MockFileProvider _fileProvider;
        [SetUp]
        public void Setup()
        {
            _fileProvider = new MockFileProvider(DummyData);
        }

        public class DummyMetaDataGenerator : IVideoMetaDataProvider
        {
            public async Task<string> GetMetaDataFromFile(string filename)
            {
                /* the following FFPMEG command will generate this information            
                */
                return @"codec_name=h264
            width=3840
            height=2160
            r_frame_rate=25/1
            duration=13.800000
            bit_rate=23204268";
            }
        }
        [Test]
        public void shouldReturnValidVideoMetaData()
        {
            VideoMetaData data = VideoTesterApi.GetVideoMetaDataAsync("/path", new DummyMetaDataGenerator()).GetAwaiter().GetResult();
            Assert.AreEqual(data.BitrateKPBS, 23204); // number is divided by 1000
            Assert.AreEqual(data.FrameRate, 25);
            Assert.AreEqual(data.Width, 3840);
            Assert.AreEqual(data.Height, 2160);
        }

        [Test]
        public void shouldGetValidConfigurations()
        {
            Dictionary<string, Configuration> configuration = VideoTesterApi.GetConfigurationsAsync("Configurations",_fileProvider).GetAwaiter().GetResult();
            Assert.AreEqual(2, configuration.Keys.Count());
            foreach (var key in configuration.Keys)
            {
                Assert.AreEqual(DummyData[key], configuration[key]);
            }

        }

        [Test]
        public void shouldMatchAConfiguration()
        {
            VideoMetaData data = VideoTesterApi.GetVideoMetaDataAsync("/path", new DummyMetaDataGenerator()).GetAwaiter().GetResult();
            
            Dictionary<string, Configuration> configuration = VideoTesterApi.GetConfigurationsAsync("Configurations", _fileProvider).GetAwaiter().GetResult();
            Dictionary<string, Configuration> results = VideoTesterApi.FindMatches(data, configuration);
            Assert.AreEqual(1, results.Keys.Count);
            foreach (var key in results.Keys)
            {
                Assert.AreEqual(DummyData[key], results[key]);
            }

        }
        [Test]
        public void shouldMatchAConfigurationUsingSimplePath()
        {
            Dictionary<string, Configuration> results = VideoTesterApi.ExtractMetaDataAndFindConfigMatchesAsync("/path", new DummyMetaDataGenerator(), _fileProvider).GetAwaiter().GetResult();
            Assert.AreEqual(1, results.Keys.Count);
            foreach (var key in results.Keys)
            {
                Assert.AreEqual(DummyData[key], results[key]);
            }
        }

    }
}

