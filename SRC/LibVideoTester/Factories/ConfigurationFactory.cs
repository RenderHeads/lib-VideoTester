using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibVideoTester.Models;
using LibVideoTester.Providers;
using LibVideoTester.Serialization;

namespace LibVideoTester.Factories {
  public class ConfigurationFactory {
    private IFileProvider _fileProvider;
    private IDeserializer<Configuration> _deserializer;
    private string[] _paths = new string[0];
    public ConfigurationFactory(IFileProvider fileProvider,
                                IDeserializer<Configuration> deserializer) {
      _fileProvider = fileProvider;
      _deserializer = deserializer;
    }

    public void ReadConfigurations(string pathToDirectory) {
      _paths = _fileProvider.GetFullPathToFilesInDirectory(pathToDirectory);
    }

    public int GetConfigurationCount() { return _paths.Length; }

    public async Task<Dictionary<string, Configuration>> GetConfigurations() {
      Dictionary<string, Configuration> c = new Dictionary<string, Configuration>();
      foreach (string path in _paths) {
        string result = await _fileProvider.GetFileContentsAsync(path);
        Configuration config;
        if (_deserializer.TryDeserialize(result, out config)) {
          c.Add(path, config);
        }
      }
      return c;
    }
  }
}
