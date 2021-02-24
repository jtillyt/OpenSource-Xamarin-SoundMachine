using System.IO;

namespace SoundMachine.Wave
{
    public class WaveMemoryStream
    {
        private readonly WaveHeader _header;
        private readonly WavefmtSubChunk _fmt;
        private WavedataSubChunk _data;

        public WaveMemoryStream(WaveFormat waveFormat)
            : this(waveFormat.Channels, waveFormat.BitsPerSample, waveFormat.SampleRate)
        {
        }

        public WaveMemoryStream(int channels, int bitsPerSample, int sampleRate)
        {
            _header = new WaveHeader();
            _fmt = new WavefmtSubChunk(channels, bitsPerSample, sampleRate);
        }

        public WaveMemoryStream(WavedataSubChunk data)
        {
            _data = data;
        }

        public void SetData(byte[] soundData, int numSamples)
        {
            _data = new WavedataSubChunk(numSamples, _fmt.NumChannels, _fmt.BitsPerSample, soundData);
        }

        public MemoryStream CreateStream()
        {
            MemoryStream memStream = new MemoryStream();

            _header.SetChunkSize(_fmt.Size, _data.Size);

            _header.WriteHeader(memStream);
            _fmt.Writefmt(memStream);
            _data.WriteData(memStream);

            return memStream;
        }

        public int NumChannels
        {
            get { return _fmt.NumChannels; }
        }

        public int BitsPerSample
        {
            get { return _fmt.BitsPerSample; }
        }
    }
}