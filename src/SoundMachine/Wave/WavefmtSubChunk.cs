using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//from https://github.com/ghoofman/WaveLibrary
namespace SoundMachine.Wave
{
    class WavefmtSubChunk
    {
        string SubChunk1ID = "fmt ";
        int Subchunk1Size = 16; //For PCM
        int AudioFormat = 1; //For no compression
        public int NumChannels = 2; //1 For Mono, 2 For Stereo
        int SampleRate = 44100;
        int ByteRate;
        int BlockAlign;
        public int BitsPerSample = 16;

        public WavefmtSubChunk(int channels, int bitsPerSamples, int sampleRate)
        {
            BitsPerSample = bitsPerSamples;
            NumChannels = channels;
            SampleRate = sampleRate;
            ByteRate = SampleRate * NumChannels * (BitsPerSample / 8);
            BlockAlign = NumChannels * (BitsPerSample / 8);
        }

        public void Writefmt(Stream stream)
        {
            //Chunk ID
            byte[] _subchunk1ID = Encoding.ASCII.GetBytes(SubChunk1ID);
            stream.Write(_subchunk1ID, 0, _subchunk1ID.Length);

            //Chunk Size
            byte[] _subchunk1Size = BitConverter.GetBytes(Subchunk1Size);
            stream.Write(_subchunk1Size, 0, _subchunk1Size.Length);

            //Audio Format (PCM)
            byte[] _audioFormat = BitConverter.GetBytes(AudioFormat);
            stream.Write(_audioFormat, 0, 2);

            //Number of Channels (1 or 2)
            byte[] _numChannels = BitConverter.GetBytes(NumChannels);
            stream.Write(_numChannels, 0, 2);

            //Sample Rate
            byte[] _sampleRate = BitConverter.GetBytes(SampleRate);
            stream.Write(_sampleRate, 0, _sampleRate.Length);

            //Byte Rate
            byte[] _byteRate = BitConverter.GetBytes(ByteRate);
            stream.Write(_byteRate, 0, _byteRate.Length);

            //Block Align
            byte[] _blockAlign = BitConverter.GetBytes(BlockAlign);
            stream.Write(_blockAlign, 0, 2);

            //Bits Per Sample
            byte[] _bitsPerSample = BitConverter.GetBytes(BitsPerSample);
            stream.Write(_bitsPerSample, 0, 2);
        }

        public int Size { get { return Subchunk1Size; } }
    }
}
