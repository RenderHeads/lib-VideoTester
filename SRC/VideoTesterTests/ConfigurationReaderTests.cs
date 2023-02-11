using NUnit.Framework;
using LibVideoTester;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
using LibVideoTester.Models;
using LibVideoTester.Factories;

namespace VideoTesterTests
{
    public class ConfigurationReaderTests
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
            {"/path1", new Configuration(new string[]{"hap","h264"}, 2048,1024,new int[]{30,60},1024 )},
            {"/path2", new Configuration(new string[]{"hevc","h264"}, 512,768,new int[]{25},2048 )}

            };
        private MockFileProvider _fileProvider;

        private IDeserializer<Configuration> _deserializer;
        [SetUp]
        public void Setup()
        {
            _fileProvider = new MockFileProvider(DummyData);
            _deserializer = new NewtonSoftJsonDeserializer<Configuration>();
        }

        [Test]
        public void shouldReturn2Configurations()
        {
            ConfigurationFactory reader = new ConfigurationFactory(_fileProvider, _deserializer);
            Assert.AreEqual(0, reader.GetConfigurationCount());
            reader.ReadConfigurations("/");
            Assert.AreEqual(2, reader.GetConfigurationCount());
        }

        [Test]
        public void shouldReturnValidConfigurationAfterReading()
        {
            ConfigurationFactory reader = new ConfigurationFactory(_fileProvider, _deserializer);
            Dictionary<string, Configuration> configuration = reader.GetConfigurations().GetAwaiter().GetResult();
            Assert.AreEqual(0, configuration.Keys.Count());

            reader.ReadConfigurations("/");
            configuration = reader.GetConfigurations().GetAwaiter().GetResult();
            Assert.AreEqual(2, configuration.Keys.Count());

            foreach (var key in configuration.Keys)
            {

                Assert.AreEqual(DummyData[key], configuration[key]);
            }

        }




    }
}
