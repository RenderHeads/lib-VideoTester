using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LibVideoTester.Providers;

namespace LibVideoTester
{
    //NOTE THIS CAN"T BE UNIT TESTED AS IT TESTS SOMETHING OUTSIDE OF OF THE PROJECT    
    public class FFProbeMetaDataProvider : IVideoMetaDataProvider
    {
        public const string EXECUTABLE_NAME = "ffprobe";

 
        public async Task<string> GetMetaDataFromFile(string filename)
        {
            var process = new Process
            {
                StartInfo = { FileName = EXECUTABLE_NAME,
                Arguments = $"-v error -select_streams v:0 -show_entries stream=width,height,duration,bit_rate,r_frame_rate,codec_name -of default=noprint_wrappers=1 \"{filename}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
                }
            };
            await Task.Run(() => process.Start());
            string result =  await process.StandardOutput.ReadToEndAsync();
            return result;

        }
    }
}

