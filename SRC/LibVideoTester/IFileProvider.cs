using System;
using System.Threading.Tasks;

namespace LibVideoTester
{
    public interface IFileProvider
    {
        public string[] GetFullPathToFilesInDirectory(string directoryPath);

        public Task<String> GetFileContentsAsync(string pathToFile);
    }
}

