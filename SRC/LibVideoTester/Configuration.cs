using System;
namespace LibVideoTester
{
    public class Configuration
    {
        private string[] _validCodecs;
        private int _maxWidth, _maxHeight;
        public int[] _frameRates;
        public int _maxBitRateKBPS;
        public Configuration(string[] validCodecs, int maxWidth, int maxHeight, int[] frameRates, int maxBirateKBPS)
        {
            _validCodecs = validCodecs;
            _maxWidth = maxHeight;
            _maxHeight = maxHeight;
            _frameRates = frameRates;
            _maxBitRateKBPS = maxBirateKBPS;

     
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

        public int[] GetFrameRates()
        {
            return _frameRates;
        }

        public int GetMaxBitRateKBPS()
        {
            return _maxBitRateKBPS;
        }

        
    }
}

