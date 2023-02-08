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

        [Test]
        public void shouldReturnTrueIfCodecIsMatch()
        {
            string [] validCodecs = new string[] { "hap", "h264","hevc", "hapa",};
            Configuration c = new Configuration(validCodecs,0,0, new int[30], 1024);
            VideoInfo v = new VideoInfo("hap",400,400,0,0);
            Assert.IsTrue(v.CodecValid(c));
            
        }


        [Test]
        public void shouldReturnTrueIfWithinValidResolution()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };
            Configuration c = new Configuration(validCodecs,500,500, new int[] { 30 }, 1024);
            VideoInfo v = new VideoInfo("hap", 400, 400, 30, 1024);
            Assert.IsTrue(v.ResolutionValid(c));
        }

        [Test]
        public void shouldReturnFalseIfHapNotDivisbleBy4()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };
            Configuration c = new Configuration(validCodecs, 500, 500, new int[] { 30 }, 1024);
            VideoInfo v = new VideoInfo("hap", 123, 400, 30, 1024);
            Assert.IsFalse(v.ResolutionValid(c));
        }

        [Test]
        public void shouldHaveValidConfig()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };
            Configuration c = new Configuration(validCodecs, 500, 500, new int[] { 30 }, 1024);
            VideoInfo v = new VideoInfo("hap", 400, 400, 30, 1024);
            Assert.IsTrue(v.TestConfiguration(c));
        }

        [Test]
        public void shouldHaveInvalidConfig()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };
            Configuration c = new Configuration(validCodecs, 200, 200, new int[] { 30}, 1024);
            VideoInfo v = new VideoInfo("hap", 400, 400, 30, 1024);
            Assert.IsFalse(v.TestConfiguration(c));
        }

        [Test]
        public void shouldReturnTrueIfFrameRateIsAMatch()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };
            Configuration c = new Configuration(validCodecs, 200, 200, new int[] { 30,60}, 1024);
            VideoInfo v = new VideoInfo("hap", 400, 400,30,1024);

            Assert.IsTrue(v.FramerateValid(c));

        }

        [Test]
        public void shouldReturnTrueIfBelowOrEqualToBitRate()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa" };
            Configuration c = new Configuration(validCodecs, 200, 200, new int[] { 30, 60 }, 1024);
            VideoInfo v = new VideoInfo("hap", 400, 400, 30, 1024);

            Assert.IsTrue(v.BitrateValid(c));

        }


    }
}
