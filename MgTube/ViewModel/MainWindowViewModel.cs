using System;
using System.Collections.Generic;
using System.Text;

namespace MgTube.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public event PlaybackResumedEventHandler PlaybackResumed;
        public event PlaybackPausedEventHandler PlaybackPaused;
        public event CurrentPositionUpdatedEventHandler CurrentPositionUpdated;

        private bool _isPlaybackActive = false;
        private string _playbackUrl;
        private TimeSpan _currentAudioTime;
        private double _currentAudioTimeSeconds;
        private double _currentAudioTotalDurationSeconds;
        private double _currentAudioTimeSliderSeconds;

        public bool IsPlaybackActive
        {
            get => _isPlaybackActive;
            private set
            {
                if (value == _isPlaybackActive) return;
                _isPlaybackActive = value;
                OnPropertyChanged();
            }
        }

        public string PlaybackUrl
        {
            get => _playbackUrl;
            set
            {
                if (value == _playbackUrl) return;
                _playbackUrl = value;
                OnPropertyChanged();
            }
        }

        public double CurrentAudioTimeSeconds
        {
            get => _currentAudioTimeSeconds;
            set
            {
                if (value.Equals(_currentAudioTimeSeconds)) return;
                _currentAudioTimeSeconds = value;
                OnPropertyChanged();
                CurrentAudioTimeSliderSeconds = value;
                CurrentPositionUpdated?.Invoke(value);
            }
        }

        public bool DoNotUpdateSliderValue { get; set; } = false;

        public double CurrentAudioTimeSliderSeconds
        {
            get => _currentAudioTimeSliderSeconds;
            set
            {
                if (DoNotUpdateSliderValue || value.Equals(_currentAudioTimeSliderSeconds)) return;
                _currentAudioTimeSliderSeconds = value;
                OnPropertyChanged();
            }
        }

        public double CurrentAudioTotalDurationSeconds
        {
            get => _currentAudioTotalDurationSeconds;
            set
            {
                if (value.Equals(_currentAudioTotalDurationSeconds)) return;
                _currentAudioTotalDurationSeconds = value;
                OnPropertyChanged();
            }
        }

        public void SwitchPlayback()
        {
            IsPlaybackActive = !IsPlaybackActive;
            if (IsPlaybackActive) PlaybackResumed?.Invoke();
            else PlaybackPaused?.Invoke();
        }
    }


    public delegate void PlaybackResumedEventHandler();
    public delegate void PlaybackPausedEventHandler();
    public delegate void CurrentPositionUpdatedEventHandler(double newPosition);
}
