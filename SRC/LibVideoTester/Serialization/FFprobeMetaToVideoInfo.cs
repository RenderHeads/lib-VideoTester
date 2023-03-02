using System;
using LibVideoTester.Models;
namespace LibVideoTester.Serialization {
  public class FFprobeMetaToVideoInfo : IDeserializer<VideoMetaData> {
    public bool TryDeserialize(string contents, out VideoMetaData metaData) {
      string[] lines = contents.Split(System.Environment.NewLine);
      int width = -1, height = -1, frameRate = -1, bitRate = -1;
      string codec = string.Empty;

      foreach (string line in lines) {
        string[] parts = line.Split('=');
        if (parts.Length == 2) {
          if (parts[0].ToLower().Contains("codec_name")) {
            codec = parts[1];
          }
          if (parts[0].ToLower().Contains("width")) {
            int.TryParse(parts[1], out width);
          }
          if (parts[0].ToLower().Contains("height")) {
            int.TryParse(parts[1], out height);
          }
          if (parts[0].ToLower().Contains("bit_rate")) {
            int.TryParse(parts[1], out bitRate);
            bitRate /= 1000;  // convert from bits to KBPS.  I thought I woudl need to divide this
                              // by 1024, but 1000 actually matches FFPROBES output?
          }
          if (parts[0].ToLower().Contains("r_frame_rate")) {
            string[] fpsSplit = parts[1].Split('/');
            int.TryParse(fpsSplit[0], out frameRate);
          }
        }
      }
      metaData = new VideoMetaData(codec, width, height, frameRate, bitRate);
      return true;
    }
  }
}
