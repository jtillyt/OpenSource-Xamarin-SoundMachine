using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Plugin.SimpleAudioPlayer;
using ReactiveUI;
using Xamarin.Forms;

namespace SoundMachine.ViewModels
{
    public abstract class SoundPlayerViewModelBase : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<bool> _isLoopedEnabled;
        private ISimpleAudioPlayer _soundPlayer;

        protected SoundPlayerViewModelBase(string displayName, string groupName)
        {
            DisplayName = displayName;
            GroupName = groupName;

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

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => this.RaiseAndSetIfChanged(ref _displayName, value);
        }

        private double _duration;
        public double Duration
        {
            get => _duration;
            set => this.RaiseAndSetIfChanged(ref _duration, value);
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set => this.RaiseAndSetIfChanged(ref _isPlaying, value);
        }

        private bool _isLooped;
        public bool IsLooped
        {
            get => _isLooped;
            set => this.RaiseAndSetIfChanged(ref _isLooped, value);
        }

        private string _groupName = "";

        public string GroupName
        {
            get { return _groupName; }
            set => this.RaiseAndSetIfChanged(ref _groupName, value);
        }

        private bool _isShown;
        public bool IsShown
        {
            get => _isShown;
            set => this.RaiseAndSetIfChanged(ref _isShown, value);
        }

        public bool IsLoopedEnabled => _isLoopedEnabled.Value;

        public abstract Stream GetAudioStream();

        protected void EnsurePlayerLoaded()
        {
            if (_soundPlayer == null)
            {
                _soundPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

                var stream = GetAudioStream();
                _soundPlayer.Load(stream);
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
                    Duration = _soundPlayer.Duration == 0 ? 1 : _soundPlayer.Duration;//Some sounds are less than one second and will be rounded down to zero on some platforms. We don't want that.
                });
            }
        }
    }
}