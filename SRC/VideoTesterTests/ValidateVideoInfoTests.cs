using NUnit.Framework;
using LibVideoTester;
namespace VideoTesterTests
{
    public class ValidateVideoInfoTests
    {
        private Configuration c;
        [SetUp]
        public void Setup()
        {
            string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa", };
            c = new Configuration(validCodecs, 500, 500, new int[] { 30,60}, 1024);
        }

        [Test]
        public void shouldReturnTrueIfCodecIsMatch()
        {
            VideoInfo v = new VideoInfo("hap",400,400,0,0);
            Assert.IsTrue(v.CodecValid(c));            
        }


        [Test]
        public void shouldReturnTrueIfWithinValidResolution()
        {
            VideoInfo v = new VideoInfo("hap", 400, 400, 30, 1024);
            Assert.IsTrue(v.ResolutionValid(c));
        }

        [Test]
        public void shouldReturnFalseIfHapNotDivisbleBy4()
        {
            VideoInfo v = new VideoInfo("hap", 123, 400, 30, 1024);
            Assert.IsFalse(v.ResolutionValid(c));
        }

        [Test]
        public void shouldHaveValidConfig()
        {
            VideoInfo v = new VideoInfo("hap", 400, 400, 30, 1024);
            Assert.IsTrue(v.TestConfiguration(c));
        }

        [Test]
        public void shouldHaveInvalidConfig()
        {
            VideoInfo v = new VideoInfo("hap", 900, 900, 30, 1024);
            Assert.IsFalse(v.TestConfiguration(c));
        }

        [Test]
        public void shouldReturnTrueIfFrameRateIsAMatch()
        {
            VideoInfo v = new VideoInfo("hap", 400, 400,30,1024);
            Assert.IsTrue(v.FramerateValid(c));
        }

        [Test]
        public void shouldReturnTrueIfBelowOrEqualToBitRate()
        {
            VideoInfo v = new VideoInfo("hap", 400, 400, 30, 1024);
            Assert.IsTrue(v.BitrateValid(c));

        }


    }
}
