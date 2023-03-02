using System;
using System.Threading.Tasks;

namespace LibVideoTester.Providers {
  // This file is not unit tested as it interfaces with the edge of the application and the system
  public class JsonFileProvider : IFileProvider {
    public async Task<string> GetFileContentsAsync(string pathToFile) {
      if (System.IO.File.Exists(pathToFile)) {
        return await System.IO.File.ReadAllTextAsync(pathToFile);
      }
      return "";
    }

    public string[] GetFullPathToFilesInDirectory(string directoryPath) {
      return System.IO.Directory.GetFiles(directoryPath, "*.json");
    }
  }
}
