using NUnit.Framework;
using LibVideoTester;
using LibVideoTester.Models;

namespace VideoTesterTests
{
public class ValidateVideoInfoTests
{
    private Configuration c;
    [SetUp]
    public void Setup()
    {
        string[] validCodecs = new string[] { "hap", "h264", "hevc", "hapa", };
        c = new Configuration("Sample Config", validCodecs, 500, 500, new int[] { 30, 60 }, 1024);
    }

    [Test]
    public void shouldReturnTrueIfCodecIsMatch()
    {
        VideoMetaData v = new VideoMetaData("hap", 400, 400, 0, 0);
        Assert.IsTrue(v.CodecValid(c));
    }


    [Test]
    public void shouldReturnTrueIfWithinValidResolution()
    {
        VideoMetaData v = new VideoMetaData("hap", 400, 400, 30, 1024);
        Assert.IsTrue(v.ResolutionValid(c));
    }

    [Test]
    public void shouldReturnFalseIfHapNotDivisbleBy4()
    {
        VideoMetaData v = new VideoMetaData("hap", 123, 400, 30, 1024);
        Assert.IsFalse(v.ResolutionValid(c));
    }

    [Test]
    public void shouldReturnFailureReasonHapNotDivisbleByFour()
    {
        VideoMetaData v = new VideoMetaData("hap", 123, 127, 30, 1024);
        ResolutionValidationFailureReason failureReason;
        Assert.IsFalse(v.ResolutionValid(c, out failureReason));
        Assert.AreEqual(ResolutionValidationFailureReason.NotDivisbleByFourWidth | ResolutionValidationFailureReason.NotDivisbleByFourHeight, failureReason);
    }

    [Test]
    public void shouldReturnFailureReasonHapNotDivisbleByFourAndWidthAndHeightTooBig()
    {
        VideoMetaData v = new VideoMetaData("hap",999999, 99999, 30, 1024);
        ResolutionValidationFailureReason failureReason;
        Assert.IsFalse(v.ResolutionValid(c, out failureReason));
        Assert.AreEqual(ResolutionValidationFailureReason.HeightTooLarge | ResolutionValidationFailureReason.WidthTooLarge | ResolutionValidationFailureReason.NotDivisbleByFourWidth | ResolutionValidationFailureReason.NotDivisbleByFourHeight, failureReason);
    }

    [Test]
    public void shouldHaveValidConfig()
    {
        VideoMetaData v = new VideoMetaData("hap", 400, 400, 30, 1024);
        Assert.IsTrue(v.TestConfiguration(c));
    }

    [Test]
    public void shouldHaveInvalidConfig()
    {
        VideoMetaData v = new VideoMetaData("hap", 900, 900, 30, 1024);
        Assert.IsFalse(v.TestConfiguration(c));
    }

    [Test]
    public void shouldReturnTrueIfFrameRateIsAMatch()
    {
        VideoMetaData v = new VideoMetaData("hap", 400, 400, 30, 1024);
        Assert.IsTrue(v.FramerateValid(c));
    }

    [Test]
    public void shouldReturnTrueIfBelowOrEqualToBitRate()
    {
        VideoMetaData v = new VideoMetaData("hap", 400, 400, 30, 1024);
        Assert.IsTrue(v.BitrateValid(c));

    }


}
}
