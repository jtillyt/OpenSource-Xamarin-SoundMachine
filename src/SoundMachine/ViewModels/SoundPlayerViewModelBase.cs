using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Linq;
using System.Reactive;
using Plugin.SimpleAudioPlayer;
using ReactiveUI;
using Xamarin.Forms;

namespace SoundMachine.ViewModels
{
    public abstract class SoundPlayerViewModelBase : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<bool> _isLoopedEnabled;

        private string _displayName;

        private double _duration;

        private string _groupName = "";

        private bool _isLooped;

        private bool _isPlaying;
        private ISimpleAudioPlayer _soundPlayer;

        protected SoundPlayerViewModelBase(string displayName, string groupName)
        {
            DisplayName = displayName;
            GroupName = groupName;

            IsWave = groupName == SoundMachineViewModel.GenWaveGroup;

            var isPlayingChanged = this.WhenAnyValue(x => x.IsPlaying);

            var canPlay = isPlayingChanged.Select(isPlaying => !isPlaying);
            var canStop = isPlayingChanged.Select(IsPlaying => IsPlaying);

            canPlay.ToProperty(this, nameof(IsLoopedEnabled), out _isLoopedEnabled)
                .DisposeWith(Disposables);

            PlayCommand = ReactiveCommand.Create(ExecutePlay, canPlay);
            StopCommand = ReactiveCommand.Create(ExecuteStop, canStop);

            PlayCommand.ObserveOn(RxApp.MainThreadScheduler)
                .Do(_ => IsPlaying = true)
                .Subscribe();

            StopCommand.ObserveOn(RxApp.MainThreadScheduler)
                .Do(_ => IsPlaying = false)
                .Subscribe();

            this.WhenAnyValue(x => x.Duration, x => x.IsPlaying, x => x.IsLooped,
                    (duration, isPlaying, isLooped) => (duration, isPlaying, isLooped))
                .Where(x => x.duration > 0 && x.isPlaying && !x.isLooped)
                .Select(x => Observable.Timer(TimeSpan.FromSeconds(x.duration)))
                .Switch()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => IsPlaying = false)
                .DisposeWith(Disposables);
        }

        public ReactiveCommand<Unit, Unit> PlayCommand { get; }

        public ReactiveCommand<Unit, Unit> StopCommand { get; }

        public string DisplayName
        {
            get => _displayName;
            set => this.RaiseAndSetIfChanged(ref _displayName, value);
        }

        public double Duration
        {
            get => _duration;
            set => this.RaiseAndSetIfChanged(ref _duration, value);
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set => this.RaiseAndSetIfChanged(ref _isPlaying, value);
        }

        public bool IsLooped
        {
            get => _isLooped;
            set => this.RaiseAndSetIfChanged(ref _isLooped, value);
        }

        public string GroupName
        {
            get => _groupName;
            set => this.RaiseAndSetIfChanged(ref _groupName, value);
        }

        public bool IsWave { get; }

        public bool IsLoopedEnabled => _isLoopedEnabled.Value;
        public event EventHandler PlayStarted;

        public abstract Stream GetAudioStream();

        protected void EnsurePlayerLoaded()
        {
            if (_soundPlayer == null)
            {
                _soundPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

                var stream = GetAudioStream();
                _soundPlayer.Load(stream);

                PlayStarted?.Invoke(this, EventArgs.Empty);
            }
        }


        private void ExecuteStop()
        {
            if (_soundPlayer != null)
                _soundPlayer.Stop();
        }

        private void ExecutePlay()
        {
            EnsurePlayerLoaded();

            _soundPlayer.Loop = IsLooped;
            _soundPlayer.Play();

            if (Duration == 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Duration = _soundPlayer.Duration == 0
                        ? 1
                        : _soundPlayer
                            .Duration; //Some sounds are less than one second and will be rounded down to zero on some platforms. We don't want that.
                });
            }
        }
    }
}