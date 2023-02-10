using System;
using System.Threading.Tasks;

namespace LibVideoTester.Providers
{
    public interface IFileProvider
    {
        public string[] GetFullPathToFilesInDirectory(string directoryPath);

        public Task<String> GetFileContentsAsync(string pathToFile);
    }
}

