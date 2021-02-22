using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Modified from https://github.com/ghoofman/WaveLibrary
namespace SoundMachine.Wave
{
    class WavedataSubChunk
    {
        string SubChunk2ID = "data";
        int SubChunk2Size;
        byte[] SoundData;

        public WavedataSubChunk(int NumSamples, int NumChannels, int BitsPerSample, byte[] SoundData)
        {
            SubChunk2Size = NumSamples * NumChannels * (BitsPerSample / 8);
            this.SoundData = SoundData;
        }

        public void WriteData(Stream stream)
        {
            //Chunk ID
            byte[] _subChunk2ID = Encoding.ASCII.GetBytes(SubChunk2ID);
            stream.Write(_subChunk2ID, 0, _subChunk2ID.Length);

            //Chunk Size
            byte[] _subChunk2Size = BitConverter.GetBytes(SubChunk2Size);
            stream.Write(_subChunk2Size, 0, _subChunk2Size.Length);

            //Wave Sound Data
            stream.Write(SoundData, 0, SoundData.Length);
        }

        public int Size { get { return SubChunk2Size; } }
    }
}
