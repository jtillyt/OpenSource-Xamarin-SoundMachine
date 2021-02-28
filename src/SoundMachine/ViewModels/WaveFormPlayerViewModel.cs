using JaybirdLabs.Chirp;
using ReactiveUI;
using System.IO;

namespace SoundMachine.ViewModels
{
    public class WaveFormPlayerViewModel : SoundPlayerViewModelBase
    {
        private readonly SignalGeneratorType _signalType;
        private readonly StreamGenerator _streamGenerator;

        public WaveFormPlayerViewModel(string displayName, string groupName, int initialFrequency, SignalGeneratorType signalType, int duration)
            : base(displayName, groupName)
        {
            _frequency = initialFrequency;
            _signalType = signalType;
            _streamGenerator = new StreamGenerator();

            Duration = duration;
        }

        private int _frequency;
        public int Frequency
        {
            get => _frequency;
            set => this.RaiseAndSetIfChanged(ref _frequency, value);
        }

        public override Stream GetAudioStream()
        {
            return _streamGenerator.GenerateStream(_signalType, 400, 5);
        }
    }
}
