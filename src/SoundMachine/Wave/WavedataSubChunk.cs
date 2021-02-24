using System;
using System.IO;
using System.Text;

//Modified from https://github.com/ghoofman/WaveLibrary
namespace SoundMachine.Wave
{
    public class WavedataSubChunk
    {
        private readonly string _subChunk2ID = "data";
        private readonly int _subChunk2Size;
        private readonly byte[] _soundData;

        public WavedataSubChunk(int NumSamples, int NumChannels, int BitsPerSample, byte[] SoundData)
        {
            _subChunk2Size = NumSamples * NumChannels * (BitsPerSample / 8);
            _soundData = SoundData;
        }

        public void WriteData(Stream stream)
        {
            //Chunk ID
            byte[] _subChunk2IDData = Encoding.ASCII.GetBytes(_subChunk2ID);
            stream.Write(_subChunk2IDData, 0, _subChunk2IDData.Length);

            //Chunk Size
            byte[] _subChunk2SizeData = BitConverter.GetBytes(this._subChunk2Size);
            stream.Write(_subChunk2SizeData, 0, _subChunk2SizeData.Length);

            //Wave Sound Data
            stream.Write(_soundData, 0, _soundData.Length);
        }

        public int Size
        {
            get { return _subChunk2Size; }
        }
    }
}