using System;
namespace LibVideoTester
{
    public class Configuration
    {
        private string[] _validCodecs;
        private int _maxWidth, _maxHeight;
        public Configuration(string[] validCodecs, int maxWidth, int maxHeight)
        {
            _validCodecs = validCodecs;
            _maxWidth = maxHeight;
            _maxHeight = maxHeight;
     
        }

        public int GetMaxWidth()
        {
            return _maxWidth;
        }

        public int GetMaxHeight()
        {
            return _maxHeight;
        }

        public string[] GetCodecs()
        {
            return _validCodecs;
        }

        
    }
}

