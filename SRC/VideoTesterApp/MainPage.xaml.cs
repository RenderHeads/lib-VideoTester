using LibVideoTester.Api;
using LibVideoTester.Models;
using LibVideoTester.Providers;
namespace VideoTesterApp;

public partial class MainPage : ContentPage
{
    private const string FFPROBE_PATH_MAC = "./Contents/Resources/ExternalBinaries/Mac/ffprobe";
    private const string FFPROBE_PATH_WINDOWS = "ffprobe"; //assume it is just next to executable?

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        GetFile();
    }

    public async Task GetFile()
    {
        try
        {
            ResultsLabel.TextColor = Color.FromRgb(255, 255, 255);
            CounterBtn.IsEnabled = false;
            var result = await FilePicker.Default.PickAsync();
            if (result != null)
            {
                CounterBtn.Text = "Please wait processing...";
                VideoMetaData v = await VideoTesterApi.GetVideoMetaDataAsync(result.FullPath, new FFProbeMetaDataProvider(DeviceInfo.Current.Platform == DevicePlatform.macOS ? FFPROBE_PATH_MAC : FFPROBE_PATH_WINDOWS));
                Dictionary<string, Configuration> configs = await VideoTesterApi.GetConfigurationsAsync("./Contents/Resources/Configurations");

                VideoMetaDataLabel.Text = $"Width: {v.Width} Height: {v.Height} Codec: {v.Codec} Framerate: {v.FrameRate} Bitrate (KPBS): {v.BitrateKPBS}";
                var matches = VideoTesterApi.FindMatches(v, configs);
                if (matches.Keys.Count == 0)
                {
                    ResultsLabel.Text = $"Out of {configs.Keys.Count} configurations found, No configuration match, this is a bad encode! do not use.";
                    ResultsLabel.TextColor = Color.FromRgb(255, 0, 0);
                }
                else
                {
                    ResultsLabel.Text = $"Out of {configs.Keys.Count} configurations found This file matches {matches.Keys.Count} configuration, it is probably safe to use.";
                    ResultsLabel.TextColor = Color.FromRgb(0, 255, 0);
                }
                CounterBtn.Text = result.FullPath;
            }
            else
            {
                CounterBtn.Text = "Something went wrong selecting a file, please try again";

            }

        }
        catch (Exception ex)
        {
            VideoMetaDataLabel.Text = ex.ToString();
        }
        CounterBtn.IsEnabled = true;
    }
}


