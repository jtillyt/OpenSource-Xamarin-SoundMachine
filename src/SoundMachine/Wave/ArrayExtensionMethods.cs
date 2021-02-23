using System;
using System.Collections.Generic;
using System.Text;

namespace SoundMachine.Wave
{
    public static class ArrayExtensionMethods
    {
        public static byte[] ToByteArray(this short[] shortArray)
        {
            byte[] bufferBytes = new byte[shortArray.Length * 2];
            Buffer.BlockCopy(shortArray, 0, bufferBytes, 0,
                bufferBytes.Length);

            return bufferBytes;
        }
    }
}
