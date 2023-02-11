using System;
using LibVideoTester.Factories;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
using NUnit.Framework;
using System.Threading.Tasks;
using LibVideoTester.Api;
namespace VideoTesterTests
{
    public class APITests
    {
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
            VideoMetaData data = VideoTesterApi.GetVideoInfo("/path", new DummyMetaDataGenerator()).GetAwaiter().GetResult();
            Assert.AreEqual(data.BitrateKPBS, 23204); // number is divided by 1000
            Assert.AreEqual(data.FrameRate, 25);
            Assert.AreEqual(data.Width, 3840);
            Assert.AreEqual(data.Height, 2160);
        }

    }
}

