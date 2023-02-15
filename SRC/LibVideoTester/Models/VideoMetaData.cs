using System;
using System.Linq;
namespace LibVideoTester.Models
{
    public class VideoMetaData
    {

        /// <summary>
        /// Possible reasons that our resolution check fails to give some insight to users.
        /// </summary>
        [System.Flags]
        public enum ResolutionValidationFailureReason
        {
            None = 0,
            NotDivisbleByFourWidth = 1,
            NotDivisbleByFourHeight = 2,
            WidthTooLarge = 4,
            HeightTooLarge = 8
        }

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
            ResolutionValidationFailureReason AppendIfNotNone(ResolutionValidationFailureReason input, ResolutionValidationFailureReason toAppend)
            {
                return input == ResolutionValidationFailureReason.None ? toAppend : input | toAppend;
            }

            failureReason = ResolutionValidationFailureReason.None;
            if (IsHap())
            {
                if (Width % 4 != 0)
                {
                    failureReason = AppendIfNotNone(failureReason, ResolutionValidationFailureReason.NotDivisbleByFourWidth);
                }

                if (Height % 4 != 0)
                {
                    failureReason = AppendIfNotNone(failureReason, ResolutionValidationFailureReason.NotDivisbleByFourHeight);
                }

            }

            if (Width > c.MaxWidth)
            {
                failureReason = AppendIfNotNone(failureReason, ResolutionValidationFailureReason.WidthTooLarge);
            }

            if (Height > c.MaxHeight)
            {
                failureReason = AppendIfNotNone(failureReason, ResolutionValidationFailureReason.HeightTooLarge);
            }

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

