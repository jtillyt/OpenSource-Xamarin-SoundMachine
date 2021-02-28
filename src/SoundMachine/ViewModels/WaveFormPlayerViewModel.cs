using System.IO;
using System.Linq;
using JaybirdLabs.Chirp;
using ReactiveUI;
using SoundMachine.ExtensionMethods;

namespace SoundMachine.ViewModels
{
    public class WaveFormPlayerViewModel : SoundPlayerViewModelBase
    {
        private readonly SignalGeneratorType _signalType;
        private readonly StreamGenerator _streamGenerator;

        private int _frequency;

        public WaveFormPlayerViewModel(string displayName, string groupName, int initialFrequency,
            SignalGeneratorType signalType, int duration)
            : base(displayName, groupName)
        {
            _frequency = initialFrequency;
            _signalType = signalType;
            _streamGenerator = new StreamGenerator();

            Duration = duration;
        }

        public int Frequency
        {
            get => _frequency;
            set => this.RaiseAndSetIfChanged(ref _frequency, value);
        }

        public short[] Amplitudes { get; private set; }

        public float[] GetAmplitudes(int count)
        {
            var amplitudes = Amplitudes.Take(count).ToArray();

            return amplitudes.ToNormalizedFloat();
        }

        public override Stream GetAudioStream()
        {
            var result = _streamGenerator.GenerateStream(_signalType, 400, 5);

            Amplitudes = result.Amplitudes;

            return result.WaveStream;
        }
    }
}