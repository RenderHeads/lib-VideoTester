using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibVideoTester
{
    public class ConfigurationReader
    {
        private IFileProvider _fileProvider;
        private IDeserializer<Configuration> _deserializer;
        private string[] _paths = new string[0];
        public ConfigurationReader(IFileProvider fileProvider, IDeserializer<Configuration> deserializer)
        {
            _fileProvider = fileProvider;
            _deserializer = deserializer;
        }

        public void ReadConfigurations(string pathToDirectory)
        {
           _paths =  _fileProvider.GetFullPathToFilesInDirectory(pathToDirectory);
        }

        public int GetConfigurationCount()
        {
            return _paths.Length;
        }

        public async Task<Dictionary<string,Configuration>> GetConfigurations()
        {
            Dictionary<string, Configuration> c = new Dictionary<string, Configuration>();  
            foreach (string path in _paths)
            {
                string result = await _fileProvider.GetFileContentsAsync(path);
                Configuration config =_deserializer.Deserialize(result);
                c.Add(path, config);
            }
            return c;
        } 
    }
}

