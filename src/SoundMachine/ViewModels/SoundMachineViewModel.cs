using ReactiveUI;
using System.Collections.ObjectModel;

namespace SoundMachine.ViewModels
{
    public class SoundMachineViewModel : ReactiveObject
    {
        public SoundMachineViewModel()
        {
            _soundPlayers = new ObservableCollection<SoundPlayerViewModelBase>()
            {
                new FilePlayerViewModel("SoundMachine.Audio.Cannon.wav", "Cannon Wave File"),
                new FilePlayerViewModel("SoundMachine.Audio.TriangleWave.wav", "Triangle Wave File"),
                new FilePlayerViewModel("SoundMachine.Audio.SawWave.wav", "Saw Wave File"),
                new FilePlayerViewModel("SoundMachine.Audio.PositiveShortDigital.flac", "Positive Sound"),
                new FilePlayerViewModel("SoundMachine.Audio.NegativeShortDigital.flac", "Negative Sound"),
                new FilePlayerViewModel("SoundMachine.Audio.BeachBird.flac", "Beach Birds"),
                new FilePlayerViewModel("SoundMachine.Audio.UrbanBird.flac", "Urban Birds"),
                new WaveFormPlayerViewModel("Sine Wave",200, Wave.SignalGeneratorType.Sin, 5),
                new WaveFormPlayerViewModel("Triangle  Wave",200, Wave.SignalGeneratorType.Triangle, 5),
                new WaveFormPlayerViewModel("Saw Wave",200, Wave.SignalGeneratorType.SawTooth, 5),
                new WaveFormPlayerViewModel("Square Wave",200, Wave.SignalGeneratorType.Square, 5),
                new WaveFormPlayerViewModel("White Noise",200, Wave.SignalGeneratorType.White, 5),
            };
        }

        private ObservableCollection<SoundPlayerViewModelBase> _soundPlayers;
        public ObservableCollection<SoundPlayerViewModelBase> SoundPlayers
        {
            get => _soundPlayers;
            set => this.RaiseAndSetIfChanged(ref _soundPlayers, value);
        }
    }
}
