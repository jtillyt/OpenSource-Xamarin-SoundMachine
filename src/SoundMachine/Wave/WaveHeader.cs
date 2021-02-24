using System;
using System.IO;
using System.Text;

//From https://github.com/ghoofman/WaveLibrary
namespace SoundMachine.Wave
{
    class WaveHeader
    {
        private readonly string _chunkID = "RIFF";
        private readonly string _format = "WAVE"; 
        private int _chunkSize = 0;

        public WaveHeader()
        {

        }

        public void SetChunkSize(int fmtSubChunkSize, int dataSubChunkSize)
        {
            _chunkSize = 4 + 8 + fmtSubChunkSize + 8 + dataSubChunkSize;
        }

        public void WriteHeader(Stream stream)
        {
            //ChunkID
            byte[] riff = Encoding.ASCII.GetBytes(_chunkID);
            stream.Write(riff, 0, riff.Length);

            //Chunk Size
            byte[] chunkSize = BitConverter.GetBytes(_chunkSize);
            stream.Write(chunkSize, 0, chunkSize.Length);

            //Data Type
            byte[] wave = Encoding.ASCII.GetBytes(_format);
            stream.Write(wave, 0, wave.Length);
        }
    }
}
