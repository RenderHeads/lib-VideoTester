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

        //TODO: Refactor this to something better, we don't actually know what hap codec will be presented as, and there are a few options of hap
        public bool IsHap()
        {
            return _codec == "hap";
        }

        public bool CodecValid(Configuration c)
        {
            return c.GetCodecs().Count(x => x == _codec) > 0;
        }

        public bool ResolutionValid(Configuration c)
        {
            bool withinRange = _width <= c.GetMaxWidth() && _height <= c.GetMaxHeight();
            if (IsHap())
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

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public int GetBitrate()
        {
            return _bitrateKPBS;
        }

        public int GetFramerate()
        {
            return _frameRate;
        }

        public string GetCodec()
        {
            return _codec;
        }
    }
}

