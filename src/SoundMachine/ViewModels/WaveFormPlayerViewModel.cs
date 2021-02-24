using ReactiveUI;
using SoundMachine.Wave;
using System;
using System.IO;

namespace SoundMachine.ViewModels
{
    public class WaveFormPlayerViewModel : SoundPlayerViewModelBase
    {
        private readonly SignalGeneratorType _signalType;

        public WaveFormPlayerViewModel(string displayName, int initialFrequency, SignalGeneratorType signalType, int duration)
            : base(displayName)
        {
            _frequency = initialFrequency;
            _signalType = signalType;

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
            var gen = new SignalGenerator(44100, 2)
            {
                Gain = 1,
                Frequency = Frequency,
                Type = _signalType
            };

            int bufferSize = (int)(gen.WaveFormat.AverageBytesPerSecond * Duration);
            int sampleCount = (int)(gen.WaveFormat.SampleRate * Duration);

            short[] waveShortData = new short[bufferSize];
            gen.Read(waveShortData, bufferSize);
            byte[] waveByteData = waveShortData.ToByteArray();

            //        ---   Uncomment to write data to a WAV file -----

            //var musicPath = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, DisplayName + ".wav");
            //var waveFile = new WaveFile(gen.WaveFormat.Channels, gen.WaveFormat.BitsPerSample, gen.WaveFormat.SampleRate);
            //waveFile.SetData(waveByteData, sampleCount);
            //waveFile.WriteFile(musicPath);

            var waveStream = new WaveMemoryStream(gen.WaveFormat);
            waveStream.SetData(waveByteData, sampleCount);
            return waveStream.CreateStream();
        }
    }
}
