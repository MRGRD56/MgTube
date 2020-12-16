using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using MgTube.ViewModel;
using MgUrlParser;

namespace MgTube.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer _soundPlayer;
        
        private MainWindowViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;

            UrlTb.AddHandler(CommandManager.ExecutedEvent, new RoutedEventHandler(PasteCommangExecuted), true);
            ViewModel.PlaybackResumed += Play;
            ViewModel.PlaybackPaused += Pause;
            ViewModel.CurrentPositionUpdated += CurrentPositionUpdated;

            CurrentAudioTimeGetter();
        }

        private async void CurrentAudioTimeGetter()
        {
            while (true)
            {
                ViewModel.CurrentAudioTimeSeconds = MainMediaElement.Position.TotalSeconds;
                if (MainMediaElement.NaturalDuration.HasTimeSpan)
                    ViewModel.CurrentAudioTotalDurationSeconds = MainMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                await Task.Delay(500);
            }
        }

        private void LoadMusic()
        {

        }

        private void Pause()
        {
            MainMediaElement.Pause();
        }

        private void Play()
        {
            MainMediaElement.Play();
        }

        private void CurrentPositionUpdated(double position)
        {
            MainMediaElement.Position = TimeSpan.FromSeconds(position);
        }

        private void PasteCommangExecuted(object sender, RoutedEventArgs e)
        {
            if (((ExecutedRoutedEventArgs) e).Command == ApplicationCommands.Paste)
            {
                if (e.Handled)
                {
                    EnterUrl();
                }
            }
        }

        private void UrlTb_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                EnterUrl();
            }
        }
        
        private void EnterUrl(bool checkFormat = true)
        {
            if (checkFormat && !Uri.IsWellFormedUriString(UrlTb.Text, UriKind.Absolute)) return;

            var mgUrl = new MgUrl(UrlTb.Text);
            ViewModel.PlaybackUrl = GetAudioDirectLink(mgUrl.Url);
            MainMediaElement.LoadedBehavior = MediaState.Manual;
            MainMediaElement.Play();
        }

        /// <summary>
        /// Возвращает кортеж ссылок. Ссылку на видео и на аудио соответственно.
        /// </summary>
        /// <returns></returns>
        private Tuple<string, string> GetVideoAllDirectLinks(string url)
        {
            var links = RunYoutubeDlProcess($"-g {url}");

            return new Tuple<string, string>(links[0], links[1]);
        }

        private string GetVideoDirectLink(string url) => GetVideoAllDirectLinks(url).Item1;

        private string GetAudioDirectLink(string url) => GetVideoAllDirectLinks(url).Item2;
        
        private List<string> RunYoutubeDlProcess(string args)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @".\Other\youtube-dl.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            var list = new List<string>();
            while (!proc.StandardOutput.EndOfStream)
            {
                list.Add(proc.StandardOutput.ReadLine());
            }

            return list;
        }

        private void AutoEnterUrl()
        {
            if (!string.IsNullOrWhiteSpace(UrlTb.Text)) return;

            if (Clipboard.ContainsText())
            {
                if (Uri.IsWellFormedUriString(Clipboard.GetText(), UriKind.Absolute))
                {
                    UrlTb.Text = Clipboard.GetText();
                    EnterUrl(false);
                }
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SwitchPlayback();
        }

        private void MainWindow_OnActivated(object? sender, EventArgs e)
        {
            AutoEnterUrl();
        }

        private void CurrentPositionSliderThumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            ViewModel.CurrentAudioTimeSeconds = CurrentPositionSlider.Value;
            ViewModel.DoNotUpdateSliderValue = false;
        }

        private void CurrentPositionSlider_OnDragStarted(object sender, DragStartedEventArgs e)
        {
            ViewModel.DoNotUpdateSliderValue = true;
        }
    }
}
