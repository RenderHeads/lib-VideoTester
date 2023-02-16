using System;
using System.Linq;
namespace LibVideoTester.Models
{
    public class VideoMetaData
    {


        public string Codec { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int BitrateKPBS { get; private set; }
        public int FrameRate { get; private set; }
        public VideoMetaData(string codec, int width, int height, int frameRate, int bitrateKBPS)
        {
            Codec = codec;
            Width = width;
            Height = height;
            FrameRate = frameRate;
            BitrateKPBS = bitrateKBPS;
        }

        //TODO: Refactor this to something better, we don't actually know what hap codec will be presented as, and there are a few options of hap
        public bool IsHap()
        {
            return Codec == "hap";
        }

        public bool CodecValid(Configuration c)
        {
            return c.ValidCodecs.Count(x => x == Codec) > 0;
        }

        public bool ResolutionValid(Configuration c)
        {
            ResolutionValidationFailureReason failureReason;
            bool result = ResolutionValid(c, out failureReason);
            return result;
        }

        public bool ResolutionValid(Configuration c, out ResolutionValidationFailureReason failureReason)
        {

            failureReason = ResolutionValidationFailureReason.None.
                AppendIfFalse(ResolutionValidationFailureReason.NotDivisbleByFourWidth, !IsHap() ? true : IsHap() && Width % 4 == 0).
                AppendIfFalse(ResolutionValidationFailureReason.NotDivisbleByFourHeight, !IsHap() ? true : IsHap() && Height % 4 == 0).
                AppendIfFalse(ResolutionValidationFailureReason.WidthTooLarge, Width <= c.MaxWidth).
                AppendIfFalse(ResolutionValidationFailureReason.HeightTooLarge, Height <= c.MaxHeight);
            return failureReason == ResolutionValidationFailureReason.None;

        }

        public bool TestConfiguration(Configuration c)
        {
            return CodecValid(c) && ResolutionValid(c) && FramerateValid(c) && BitrateValid(c);
        }

        public bool FramerateValid(Configuration c)
        {
            return c.FrameRates.Count(x => FrameRate == x) > 0;
        }

        public bool BitrateValid(Configuration c)
        {
            return BitrateKPBS <= c.MaxBitRate;
        }


    }
}

