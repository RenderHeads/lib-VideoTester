using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LibVideoTester.Providers
{
//NOTE THIS CAN"T BE UNIT TESTED AS IT TESTS SOMETHING OUTSIDE OF OF THE PROJECT
public class FFProbeMetaDataProvider : IVideoMetaDataProvider
{
    private string _executablePath;
    public FFProbeMetaDataProvider(string executablePath = "ffprobe")
    {
        _executablePath = executablePath;
    }

    public async Task<string> GetMetaDataFromFile(string filename)
    {
        var process = new Process
        {
            StartInfo = {
                FileName = _executablePath,
                Arguments = $"-v error -select_streams v:0 -show_entries stream=width,height,duration,bit_rate,r_frame_rate,codec_name -of default=noprint_wrappers=1 \"{filename}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        await Task.Run(() => process.Start());
        string result = await process.StandardOutput.ReadToEndAsync();
        return result;

    }
}

}