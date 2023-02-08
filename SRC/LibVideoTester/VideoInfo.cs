using System;
using System.Linq;
namespace LibVideoTester
{
    public class VideoInfo
    {
        private string _codec;
        private int _width;
        private int _height;
        public VideoInfo(string codec, int width, int height)
        {
            _codec = codec;
            _width = width;
            _height = height;
        }


        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
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
    }
}

