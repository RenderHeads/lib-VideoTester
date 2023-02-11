
<p align="center">
  <img src="https://renderheads-file-share.s3.af-south-1.amazonaws.com/assets/renderheads.svg" width=50%>
</p>
<p align="center">
  <b>RenderHeads ©2023</b>
</p>
<p align ="center"> Author / Maintainer: Shane Marks </p>


# Video Sanity Testing Tool
This project is being built to allow our internal and external team members to  quickly check if a video file matches a specific configuration that we need, to ensure the video will work in the game / application it has been made for. The intention is to catch problems with files early on in the process and prevent slow downs later on in the development process.

## Principle Design:
FFProbe can provided us with meta data in a format that is pretty easy to parse. Given the complicated nature of the syntax, this acts as a wrapper tool that  generally simplifies things for the non power-user.

For example the following command:
```
ffprobe -v error -select_streams v:0 -show_entries stream=width,height,duration,bit_rate,r_frame_rate,codec_name -of default=noprint_wrappers=1 myvideo.mov
```

will return an output as follows:
```
codec_name=h264
width=3840
height=2160
r_frame_rate=25/1
duration=13.800000
bit_rate=23204268
```

This is pretty easy to then parse and compare against some known configuration.  This project provides an API to get these details, as well as compare them against configurations. It also commands a CLI tool that implements the API

Configurations are sepcific in json in a ```Configurations``` folder as in the below example:
Setup configurations in the configurations folder as per your requirements.  These files need to be valid JSON
```
{
  "ValidCodecs": [ "hap", "h264" ],
  "MaxWidth": 2048,
  "MaxHeight": 1024,
  "FrameRates": [ 30, 60 ],
  "MaxBitRate": 1024
}
```
- ValidCodec: an array of codec names that are valid
- MaxWidth: the largest the video width can be.
- MaxHeight: the largest the video height can be.
- FrameRates: An array of integer values for acceptable framerates.
- Max Bitrate:  The highest acceptable bitrack in KBPS.


# Dependencies
This project requries *ffprobe* (part of ffmpeg package) installed on your compmuter and available in PATH. We  will probably  want to build this in at some point.
## Installing FFProbe
- Mac (CLI): ```brew install ffmpeg```
- Windows: Download here: https://ffmpeg.org/download.html - install and add to path manually.
- Ubuntu(CLI): ```sudo apt install ffmpeg```

# Project Structure
The project is made up out of a console app, a tests project and a library.

# Things it can check
- Width, Height, Codec, Bit Rate, Frame Rate

# Example Usage
Currently it is just configured to allow h264 files to pass.  In the demo below, the first video passes, the second video fails.

<img width="1115" alt="image" src="https://user-images.githubusercontent.com/18391483/218257157-f35af1f6-3bf5-4b83-bb05-1ab504bc04a6.png">

# Known Issues:
HAP Codec files width and height, need to be divisble by 4, however the logic to check if we have HAP codec specified, is currently flawed as it doesn't account for all the different flavours of HAP.



# Low hanging fruit
Some things that would be easy to PR in, if someone was up to it
-  Upgrade  CLI and test project from dotnet Core 3.1 to 6.
-  Handle corrupted Json files
- ~~Neaten up files locations and interfaces~~
- ~~Separate out logic for parsing FFMPEG meta data~~
- ~~Start documenting library API before it gets out of hand~~
- ~~Read config from json~~ (it will try parse all JSON files in the Configurations directory).
- ~~Rename the folder for the Console project (it is badly named).~~
- ~~Better argument handling.~~  (Using System.Commandline dotNet preview package by microsoft)
- Add a CI pipeline to Auto Build.
- Add a built in version of ffprobe to prevent need of  pre installed dependecy

- Add an option to generate a report and save it next to the checked file.

# Limitations
- We only check the first stream of the video file.
- We don't get audio data and check that
# Next Steps
- Clean up formatting, add linting rules
- Add CI Pipeline to auto build
- Add in library for better command line argument handling
- Add check for ffprobe on path and if it can't find it print an error
- Add flag to save report to file
- Build little GUI with dotNet MAUI
  - Make path to ffprobe configurable?

# Command line Usage
```
./VideoTesterConsoleApp -i YOUR_FILE_HERE.EXTENSION
```


# Api
The Api can be found in **SRC/LibVideoTester/Api/Api.cs**

Usage 1) The One Liner
```
Dictionary<string, Configuration> results = await VideoTesterApi.ExtractMetaDataAndFindConfigMatchesAsync("/movie.mov");
if (results.Keys.Count > 0){
  //you have a  configuration that matches!!
}
```
Usage 2) Do it in steps for more control
```
//1) Get our video meta data
VideoMetaData data = await VideoTesterApi.GetVideoMetaDataAsync("/movie.mov");          

//2) Read our configurations from directory
Dictionary<string, Configuration> configuration = await VideoTesterApi.GetConfigurationsAsync();

//3) Check for matches
Dictionary<string, Configuration> results = VideoTesterApi.FindMatches(data, configuration);
```

Usage 3) Advanced (changing default behaviour)
```
//This will  override default behaviour and isntead of reading from FFProbe, it will just use some dummy data, as per our tests
VideoMetaData data = await VideoTesterApi.GetVideoMetaDataAsync("/movie.mov",new DummyMetaDataGenerator());      

//In this contrived example we will fetch configs from AWS S3 instead of disk, and replace our json deserializer with a Yaml deserializer
Dictionary<string, Configuration> configuration = VideoTesterApi.GetConfigurationsAsync("Configurations", new S3FileProvider(), new YamlDeserializer<Configuration>())


```



# Usage and Contribution
## Usage License
The project is licensed under a GPL-3.0 license, which means you can use it for commerical and non commercial use, but the project you use it in also needs to apply the GPL-3.0 license and be open-sourced. If this license is not suitable for you, please contact us at southafrica@renderheads.com and we can discuss an appropriate commercial license.

## Contributors License
We are currently working on a Contributors License Agreement, which we will put up here when it's ready. In the meantime, if you would like to contribute, please reach out to us.
  
