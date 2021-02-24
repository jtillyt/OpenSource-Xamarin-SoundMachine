using System;
using System.IO;
using System.Text;

//from https://github.com/ghoofman/WaveLibrary
namespace SoundMachine.Wave
{
    public class WavefmtSubChunk
    {
        private readonly string _subChunk1ID = "fmt ";
        private readonly int _subchunk1Size = 16; //For PCM
        private readonly int _audioFormat = 1; //For no compression
        private readonly int _sampleRate;
        private readonly int _byteRate;
        private readonly int _blockAlign;

        public int NumChannels { get; } //1 For Mono, 2 For Stereo
        public int BitsPerSample { get; }

        public WavefmtSubChunk(int channels, int bitsPerSamples, int sampleRate)
        {
            BitsPerSample = bitsPerSamples;
            NumChannels = channels;

            _sampleRate = sampleRate;
            _byteRate = _sampleRate * NumChannels * (BitsPerSample / 8);
            _blockAlign = NumChannels * (BitsPerSample / 8);
        }

        public void Writefmt(Stream stream)
        {
            //Chunk ID
            byte[] _subchunk1IDData = Encoding.ASCII.GetBytes(_subChunk1ID);
            stream.Write(_subchunk1IDData, 0, _subchunk1IDData.Length);

            //Chunk Size
            byte[] _subchunk1SizeData = BitConverter.GetBytes(_subchunk1Size);
            stream.Write(_subchunk1SizeData, 0, _subchunk1SizeData.Length);

            //Audio Format (PCM)
            byte[] _audioFormatData = BitConverter.GetBytes(_audioFormat);
            stream.Write(_audioFormatData, 0, 2);

            //Number of Channels (1 or 2)
            byte[] _numChannelsData = BitConverter.GetBytes(NumChannels);
            stream.Write(_numChannelsData, 0, 2);

            //Sample Rate
            byte[] _sampleRateData = BitConverter.GetBytes(_sampleRate);
            stream.Write(_sampleRateData, 0, _sampleRateData.Length);

            //Byte Rate
            byte[] _byteRateData = BitConverter.GetBytes(_byteRate);
            stream.Write(_byteRateData, 0, _byteRateData.Length);

            //Block Align
            byte[] _blockAlignData = BitConverter.GetBytes(_blockAlign);
            stream.Write(_blockAlignData, 0, 2);

            //Bits Per Sample
            byte[] _bitsPerSampleData = BitConverter.GetBytes(BitsPerSample);
            stream.Write(_bitsPerSampleData, 0, 2);
        }

        public int Size
        {
            get { return _subchunk1Size; }
        }
    }
}