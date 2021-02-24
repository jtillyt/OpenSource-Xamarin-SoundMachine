using System;

namespace SoundMachine.Wave
{
    //Based on https://github.com/naudio/NAudio/blob/master/NAudio.Core/Wave/SampleProviders/SignalGenerator.cs
    public class SignalGenerator
    {
        private readonly WaveFormat _waveFormat;
        private readonly Random _random = new Random();

        public SignalGenerator()
            : this(44100, 2)
        {
        }

        /// <summary>
        /// Initializes a new instance for the Generator (UserDef SampleRate &amp; Channels)
        /// </summary>
        /// <param name="sampleRate">Desired sample rate</param>
        /// <param name="channel">Number of channels</param>
        public SignalGenerator(int sampleRate, int channel)
        {
            _waveFormat = new WaveFormat(sampleRate, channel);

            // Default
            Type = SignalGeneratorType.Sin;
            Frequency = 440.0;
            Gain = 1;
        }

        public WaveFormat WaveFormat => _waveFormat;

        public double Frequency { get; set; }

        public double Gain { get; set; }

        public SignalGeneratorType Type { get; set; }

        /// <summary>
        /// Reads from this provider.
        /// </summary>
        public int Read(short[] buffer, int count)
        {
            if (Gain > 1.0)
                Gain = 1.0;

            short amplitude = (short) (short.MaxValue * (short) Gain);

            for (uint index = 0; index < count - 1; index++)
            {
                double timePeriod;
                double timeIndex;
                double modResult;
                double sampleDouble;
                short sampleValue;
                switch (Type)
                {
                    case SignalGeneratorType.Sin:
                        timePeriod = (Math.PI * Frequency) / (WaveFormat.SampleRate);
                        sampleValue = Convert.ToInt16(amplitude * Math.Sin(timePeriod * index));
                        break;
                    case SignalGeneratorType.Square:
                        timePeriod = (Frequency) / _waveFormat.SampleRate;
                        timeIndex = index * timePeriod;
                        modResult = timeIndex % 2;
                        sampleDouble = modResult - 1;
                        sampleValue = sampleDouble > 0 ? amplitude : (short) (amplitude * -1);
                        break;
                    case SignalGeneratorType.Triangle:
                        timePeriod = Frequency / _waveFormat.SampleRate;
                        timeIndex = index * timePeriod;
                        modResult = timeIndex % 2;
                        sampleDouble = modResult * 2;
                        if (sampleDouble > 1)
                            sampleDouble = (2 - sampleDouble);
                        if (sampleDouble < -1)
                            sampleDouble = (-2 - sampleDouble);
                        sampleValue = Convert.ToInt16(sampleDouble * amplitude);
                        break;
                    case SignalGeneratorType.SawTooth:
                        timePeriod = Frequency / _waveFormat.SampleRate;
                        timeIndex = index * timePeriod;
                        modResult = timeIndex % 2;
                        sampleDouble = modResult - 1;
                        sampleValue = Convert.ToInt16(amplitude * sampleDouble);
                        break;
                    case SignalGeneratorType.White:
                        sampleValue = Convert.ToInt16(amplitude * _random.NextDouble() - 1);
                        break;
                    default:
                        sampleValue = 0;
                        break;
                }

                buffer[index] = sampleValue;
            }

            return count;
        }
    }
}