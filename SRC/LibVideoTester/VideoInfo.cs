using System;
using System.Linq;
namespace LibVideoTester
{
    public class VideoInfo
    {
        private string _codec;
        private int _width;
        private int _height;
        private int _bitrateKPBS;
        private int _frameRate;
        public VideoInfo(string codec, int width, int height, int frameRate, int bitrateKBPS)
        {
            _codec = codec;
            _width = width;
            _height = height;
            _frameRate = frameRate;
            _bitrateKPBS = bitrateKBPS;
        }


        public bool CodecValid(Configuration c)
        {
            return c.GetCodecs().Count(x => x == _codec) > 0;
        }

        public bool ResolutionValid(Configuration c)
        {
            bool withinRange = _width <= c.GetMaxWidth() && _height <= c.GetMaxHeight();
            if (_codec == "hap")
            {
                return _width % 4 == 0 && _height % 4 == 0 && withinRange;
            }
            return withinRange;
          
        }

        public bool TestConfiguration(Configuration c)
        {
            return CodecValid(c) && ResolutionValid(c) && FramerateValid(c) && BitrateValid(c);
        }

        public bool FramerateValid(Configuration c)
        {
            return c.GetFrameRates().Count(x => _frameRate == x) > 0;
        }

        public bool BitrateValid(Configuration c)
        {
            return _bitrateKPBS <= c.GetMaxBitRateKBPS();
        }
    }
}

