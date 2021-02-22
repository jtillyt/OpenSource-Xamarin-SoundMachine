using System;
using System.Collections.Generic;
using System.Text;

namespace SoundMachine.Wave
{
    public static class FloatToByteConverter
    {
        public static byte[] ToByteArray(this float[] floatArray)
        {
            byte[] buffer = new byte[floatArray.Length * 4];

            for (int i = 0; i < floatArray.Length; i++)
            {
                float d = floatArray[i];
                byte[] convertedData = BitConverter.GetBytes(d);
                Array.Copy(convertedData, 0, buffer, i * 4, 4);
            }

            return buffer;
        }
    }
}
