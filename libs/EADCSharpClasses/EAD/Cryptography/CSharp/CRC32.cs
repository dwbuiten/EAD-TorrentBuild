namespace EAD.Cryptography.CSharp
{
    using System;
    using System.IO;

    public class CRC32
    {
        private const int BUFFER_SIZE = 0x400;
        private uint[] crc32Table;

        public CRC32()
        {
            uint num = 0xedb88320;
            this.crc32Table = new uint[0x100];
            for (uint i = 0; i < 0x100; i++)
            {
                uint num4 = i;
                for (uint j = 8; j > 0; j--)
                {
                    if ((num4 & 1) == 1)
                    {
                        num4 = (num4 >> 1) ^ num;
                    }
                    else
                    {
                        num4 = num4 >> 1;
                    }
                }
                this.crc32Table[i] = num4;
            }
        }

        public uint GetCrc32(Stream stream)
        {
            uint maxValue = uint.MaxValue;
            byte[] buffer = new byte[0x400];
            int count = 0x400;
            for (int i = stream.Read(buffer, 0, count); i > 0; i = stream.Read(buffer, 0, count))
            {
                for (int j = 0; j < i; j++)
                {
                    maxValue = (maxValue >> 8) ^ this.crc32Table[(int) ((IntPtr) (buffer[j] ^ (maxValue & 0xff)))];
                }
            }
            return ~maxValue;
        }
    }
}

