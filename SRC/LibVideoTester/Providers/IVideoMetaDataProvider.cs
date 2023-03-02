using System;
using System.Threading.Tasks;

namespace LibVideoTester.Providers {
  public interface IVideoMetaDataProvider {  /// <summary>
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
}
