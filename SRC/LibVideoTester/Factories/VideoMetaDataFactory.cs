using System;
using System.Linq;
using System.Threading.Tasks;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;
namespace LibVideoTester.Factories
{

    public class VideoMetaDataFactory
    {
        private IVideoMetaDataProvider _metaDataGenerator;
        private IDeserializer<VideoMetaData> _deserializer;
        public VideoMetaDataFactory(IVideoMetaDataProvider metaDataGenerator, IDeserializer<VideoMetaData> deserializer)
        {
            _metaDataGenerator = metaDataGenerator;
            _deserializer = deserializer;
        }


        public async Task<VideoMetaData> GetVideoInfoAsync(string filename)
        {
            string metaData = await _metaDataGenerator.GetMetaDataFromFile(filename);
            VideoMetaData metaDataObject = default(VideoMetaData);
            _deserializer.TryDeserialize(metaData, out metaDataObject);
            return metaDataObject;
        }
    }
}

