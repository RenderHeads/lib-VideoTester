using NUnit.Framework;
using LibVideoTester;
namespace VideoTesterTests
{
    public class ValidateVideoInfoTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //We need some configuration for what we should check against?
        [Test]
        public void shouldReturnTrueIfCodecIsMatch()
        {
            string [] validCodecs = new string[] { "hap", "h264","hevc", "hapa"};

            Configuration c = new Configuration(validCodecs,0,0);

            VideoInfo v = new VideoInfo("hap",400,400);

            Assert.IsTrue(v.CodecValid(c));
            
        }

        //We need some configuration for what we should check against?
        [Test]
        public void shouldReturnTrueIfHapAndFileSizeDivizibleBy4()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };

            Configuration c = new Configuration(validCodecs,500,500);

            VideoInfo v = new VideoInfo("hap", 400,400);

            Assert.IsTrue(v.ResolutionValid(c));
        }

        [Test]
        public void shouldReturnTrueIfWithinValidResolution()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };

            Configuration c = new Configuration(validCodecs,500,500);

            VideoInfo v = new VideoInfo("mp4", 400, 400);

            Assert.IsTrue(v.ResolutionValid(c));
        }


    }
}
