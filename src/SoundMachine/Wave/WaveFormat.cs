using System;
using System.Collections.Generic;
using System.Text;

namespace SoundMachine.Wave
{
    //Copied parts from https://github.com/naudio/NAudio/blob/master/NAudio.Core/Wave/WaveFormats/WaveFormat.cs
    public class WaveFormat
    {
        protected WaveFormatEncoding _waveFormatTag;
        protected short _channels;
        protected int _sampleRate;
        protected int _averageBytesPerSecond;
        protected short _blockAlign;
        protected short _bitsPerSample;
        protected short _extraSize;

        /// <summary>
        /// Creates a new PCM 44.1Khz stereo 16 bit format
        /// </summary>
        public WaveFormat() 
            : this(44100, 16, 2)
        {

        }

        /// <summary>
        /// Creates a new 16 bit wave format with the specified sample
        /// rate and channel count
        /// </summary>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of channels</param>
        public WaveFormat(int sampleRate, int channels)
            : this(sampleRate, 16, channels)
        {
        }

        /// <summary>
        /// Creates a new PCM format with the specified sample rate, bit depth and channels
        /// </summary>
        public WaveFormat(int rate, int bits, int channels)
        {
            if (channels < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(channels), "Channels must be 1 or greater");
            }

            // minimum 16 bytes, sometimes 18 for PCM
            _waveFormatTag = WaveFormatEncoding.Pcm;
            _channels = (short)channels;
            _sampleRate = rate;
            _bitsPerSample = (short)bits;
            _extraSize = 0;

            _blockAlign = (short)(channels * (bits / 8));
            _averageBytesPerSecond = this._sampleRate * this._blockAlign;
        }

        /// <summary>
        /// Creates a WaveFormat with custom members
        /// </summary>
        /// <param name="tag">The encoding</param>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of channels</param>
        /// <param name="averageBytesPerSecond">Average Bytes Per Second</param>
        /// <param name="blockAlign">Block Align</param>
        /// <param name="bitsPerSample">Bits Per Sample</param>
        /// <returns></returns>
        public static WaveFormat CreateCustomFormat(WaveFormatEncoding tag, int sampleRate, int channels, int averageBytesPerSecond, int blockAlign, int bitsPerSample)
        {
            WaveFormat waveFormat = new WaveFormat
            {
                _waveFormatTag = tag,
                _channels = (short)channels,
                _sampleRate = sampleRate,
                _averageBytesPerSecond = averageBytesPerSecond,
                _blockAlign = (short)blockAlign,
                _bitsPerSample = (short)bitsPerSample,
                _extraSize = 0
            };

            return waveFormat;
        }

        /// <summary>
        /// Creates an A-law wave format
        /// </summary>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of Channels</param>
        /// <returns>Wave Format</returns>
        public static WaveFormat CreateALawFormat(int sampleRate, int channels)
        {
            return CreateCustomFormat(WaveFormatEncoding.ALaw, sampleRate, channels, sampleRate * channels, channels, 8);
        }

        /// <summary>
        /// Creates a Mu-law wave format
        /// </summary>
        /// <param name="sampleRate">Sample Rate</param>
        /// <param name="channels">Number of Channels</param>
        /// <returns>Wave Format</returns>
        public static WaveFormat CreateMuLawFormat(int sampleRate, int channels)
        {
            return CreateCustomFormat(WaveFormatEncoding.MuLaw, sampleRate, channels, sampleRate * channels, channels, 8);
        }

        /// <summary>
        /// Creates a new 32 bit IEEE floating point wave format
        /// </summary>
        /// <param name="sampleRate">sample rate</param>
        /// <param name="channels">number of channels</param>
        public static WaveFormat CreateIeeeFloatWaveFormat(int sampleRate, int channels)
        {
            var wf = new WaveFormat
            {
                _waveFormatTag = WaveFormatEncoding.IeeeFloat,
                _channels = (short)channels,
                _bitsPerSample = 32,
                _sampleRate = sampleRate,
                _blockAlign = (short)(4 * channels),
                _extraSize = 0
            };
            wf._averageBytesPerSecond = sampleRate * wf._blockAlign;
            return wf;
        }

        /// <summary>
        /// Returns the number of channels (1=mono,2=stereo etc)
        /// </summary>
        public int Channels => _channels;

        /// <summary>
        /// Returns the sample rate (samples per second)
        /// </summary>
        public int SampleRate => _sampleRate;

        /// <summary>
        /// Returns the average number of bytes used per second
        /// </summary>
        public int AverageBytesPerSecond => _averageBytesPerSecond;

        /// <summary>
        /// Returns the block alignment
        /// </summary>
        public virtual int BlockAlign => _blockAlign;

        /// <summary>
        /// Returns the number of bits per sample (usually 16 or 32, sometimes 24 or 8)
        /// Can be 0 for some codecs
        /// </summary>
        public int BitsPerSample => _bitsPerSample;

        /// <summary>
        /// Returns the number of extra bytes used by this waveformat. Often 0,
        /// except for compressed formats which store extra data after the WAVEFORMATEX header
        /// </summary>
        public int ExtraSize => _extraSize;
    }
}
