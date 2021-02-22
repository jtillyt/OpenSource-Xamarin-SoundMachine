using ReactiveUI;
using SoundMachine.Wave;
using System;
using System.IO;

namespace SoundMachine.ViewModels
{
    public class WaveFormPlayerViewModel : SoundPlayerViewModelBase
    {
        private readonly SignalGeneratorType _signalType;
        private string _displayName;
        public WaveFormPlayerViewModel(string displayName, int initialFrequency, SignalGeneratorType signalType, int duration)
            : base(displayName)
        {
            Duration = duration;

            _frequency = initialFrequency;
            _signalType = signalType;
            _displayName = displayName;
        }

        private int _frequency;
        public int Frequency
        {
            get => _frequency;
            set => this.RaiseAndSetIfChanged(ref _frequency, value);
        }

        public override Stream GetAudioStream()
        {
            var gen = new SignalGenerator(500,2)
            {
                Gain = 0.9,
                Frequency = Frequency,
                Type = _signalType
            };

            double len = (gen.WaveFormat.AverageBytesPerSecond * Duration);
            float[] data = new float[(int)len];
            gen.Read(data, 0, data.Length);

            //var musicPath = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, _displayName+".wav");

            //var waveFile = new WaveFile(gen.WaveFormat.Channels, gen.WaveFormat.BitsPerSample, gen.WaveFormat.SampleRate);
            //waveFile.SetData(data.ToByteArray(), (int)(gen.WaveFormat.SampleRate * Duration));
            //waveFile.WriteFile(musicPath);

            var waveStream = new WaveMemoryStream(gen.WaveFormat.Channels, gen.WaveFormat.BitsPerSample, gen.WaveFormat.SampleRate);
            waveStream.SetData(data, (int)(gen.WaveFormat.SampleRate * Duration));
            return waveStream.CreateStream();
        }
    }
}
