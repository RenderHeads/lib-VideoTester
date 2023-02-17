using System;
using System.Threading.Tasks;
using LibVideoTester;
using NUnit.Framework;
using LibVideoTester.Providers;
using LibVideoTester.Factories;
using LibVideoTester.Models;
using LibVideoTester.Serialization;
namespace VideoTesterTests
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
public class VideoInfoGenerationTests
{
    [Test]
    public void shouldReturnValidVideoInfo()
    {
        VideoMetaDataFactory generator = new VideoMetaDataFactory(new DummyMetaDataGenerator(), new FFprobeMetaToVideoInfo());
        VideoMetaData v = generator.GetVideoInfoAsync("c:/foo").GetAwaiter().GetResult(); // the file path doesn't matter
        Assert.IsNotNull(v);
        Configuration c = new Configuration("Sample Config", new string[] { "h264" }, 3840, 2160, new int[] { 25 }, 23205);
        Assert.IsTrue(v.BitrateValid(c));
        Assert.IsTrue(v.CodecValid(c));
        Assert.IsTrue(v.FramerateValid(c));
        Assert.IsTrue(v.ResolutionValid(c));
        Assert.IsTrue(v.TestConfiguration(c));
    }

}
}

