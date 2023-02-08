using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibVideoTester
{
    public interface IMetaDataGenerator
    {/// <summary>
    /// Needs to generate metadata like this, which you can get from a tool like ffprobe:
    /// codec_name=h264
    /// width=3840
    /// height=2160
    /// r_frame_rate=25/1
    /// duration=13.800000
    /// bit_rate=23204268"
    /// </summary>
    /// <param name="filename">the file name to check against</param>
    /// <returns></returns>
        Task<string> GetMetaDataFromFile(string filename);
    }

    public class VideoInfoGenerator
    {
        private IMetaDataGenerator _metaDataGenerator;
        public VideoInfoGenerator(IMetaDataGenerator metaDataGenreator)
        {
            _metaDataGenerator = metaDataGenreator;
        }

        public async Task<VideoInfo> GetVideoInfo(string filename)
        {
            string medataData = await _metaDataGenerator.GetMetaDataFromFile(filename);
            string[] lines = medataData.Split(System.Environment.NewLine);
            int width=-1, height=-1, frameRate=-1, bitRate = -1;
            string codec = string.Empty;
            foreach (string line in lines)
            {
                string[]parts = line.Split('=');
                if (parts.Length == 2)
                {
                    if (parts[0].ToLower().Contains("codec_name"))
                    {
                        codec = parts[1];
                    }
                    if (parts[0].ToLower().Contains("width"))
                    {
                        int.TryParse(parts[1], out width);
                    }
                    if (parts[0].ToLower().Contains("height"))
                    {
                        int.TryParse(parts[1], out height);
                    }
                    if (parts[0].ToLower().Contains("bit_rate"))
                    {
                        int.TryParse(parts[1], out bitRate);
                        bitRate /= 1000; //convert from bits to KBPS.  I thought I woudl need to divide this by 1024, but 1000 actually matches FFPROBES output?
                    }
                    if (parts[0].ToLower().Contains("r_frame_rate"))
                    {
                        string[] fpsSplit = parts[1].Split('/');
                        int.TryParse(fpsSplit[0], out frameRate );
                    }
                }
            }
            return new VideoInfo(codec,width,height,frameRate,bitRate);
        }
    }
}

