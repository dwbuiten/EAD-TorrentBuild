namespace EAD.Cryptography.ThexCS
{
    using System;
    using System.IO;

    public class ThexOptimized
    {
        private const int Block_Size = 0x80;
        private FileStream FilePtr;
        private byte[][] HashValues;
        private int Leaf_Count;
        private const int Leaf_Size = 0x400;

        private byte[] ByteExtract(byte[] Raw_Data, int Data_Length)
        {
            byte[] buffer = new byte[Data_Length];
            for (int i = 0; i < Data_Length; i++)
            {
                buffer[i] = Raw_Data[i];
            }
            return buffer;
        }

        private byte[] CompressHashBlock(byte[][] HashBlock, int HashCount)
        {
            if (HashBlock.Length != 0)
            {
                while (HashCount > 1)
                {
                    int num = HashCount / 2;
                    if ((HashCount % 2) > 0)
                    {
                        num++;
                    }
                    byte[][] bufferArray = new byte[num][];
                    int index = 0;
                    for (int i = 0; i < (HashCount / 2); i++)
                    {
                        bufferArray[i] = this.InternalHash(HashBlock[index], HashBlock[index + 1]);
                        index += 2;
                    }
                    if ((HashCount % 2) > 0)
                    {
                        bufferArray[num - 1] = HashBlock[HashCount - 1];
                    }
                    HashBlock = bufferArray;
                    HashCount = num;
                }
                return HashBlock[0];
            }
            return null;
        }

        private byte[] CompressSmallBlock()
        {
            long num = this.FilePtr.Length - this.FilePtr.Position;
            int hashCount = ((int) num) / 0x800;
            if ((num % 0x800L) > 0L)
            {
                hashCount++;
            }
            byte[][] hashBlock = new byte[hashCount][];
            for (int i = 0; i < (((int) num) / 0x800); i++)
            {
                hashBlock[i] = this.GetNextLeafHash();
            }
            if ((num % 0x800L) > 0L)
            {
                hashBlock[hashCount - 1] = this.GetNextLeafHash();
            }
            this.FilePtr.Close();
            return this.CompressHashBlock(hashBlock, hashCount);
        }

        private void GetLeafHash()
        {
            int num2 = this.Leaf_Count / 0x100;
            if ((this.Leaf_Count % 0x100) > 0)
            {
                num2++;
            }
            this.HashValues = new byte[num2][];
            byte[][] hashBlock = new byte[0x80][];
            int index = 0;
            while (index < (this.Leaf_Count / 0x100))
            {
                for (int i = 0; i < 0x80; i++)
                {
                    hashBlock[i] = this.GetNextLeafHash();
                }
                this.HashValues[index] = this.CompressHashBlock(hashBlock, 0x80);
                index++;
            }
            if (index < num2)
            {
                this.HashValues[index] = this.CompressSmallBlock();
            }
            this.Leaf_Count = num2;
            this.FilePtr.Close();
        }

        private byte[] GetNextLeafHash()
        {
            byte[] buffer = new byte[0x400];
            byte[] buffer2 = new byte[0x400];
            int num = this.FilePtr.Read(buffer, 0, 0x400);
            if (num < 0x400)
            {
                return this.LeafHash(this.ByteExtract(buffer, num));
            }
            num = this.FilePtr.Read(buffer2, 0, 0x400);
            if (num < 0x400)
            {
                buffer2 = this.ByteExtract(buffer2, num);
            }
            buffer = this.LeafHash(buffer);
            buffer2 = this.LeafHash(buffer2);
            return this.InternalHash(buffer, buffer2);
        }

        public byte[] GetTTH(string Filename)
        {
            try
            {
                byte[] buffer = null;
                if (!File.Exists(Filename))
                {
                    return null;
                }
                this.FilePtr = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (this.FilePtr.Length <= 0x20000L)
                {
                    buffer = this.CompressSmallBlock();
                }
                else
                {
                    this.Leaf_Count = ((int) this.FilePtr.Length) / 0x400;
                    if ((this.FilePtr.Length % 0x400L) > 0L)
                    {
                        this.Leaf_Count++;
                    }
                    this.GetLeafHash();
                    buffer = this.CompressHashBlock(this.HashValues, this.Leaf_Count);
                }
                return buffer;
            }
            catch (Exception)
            {
                this.FilePtr.Close();
                return null;
            }
        }

        private byte[] InternalHash(byte[] LeafA, byte[] LeafB)
        {
            byte[] array = new byte[(LeafA.Length + LeafB.Length) + 1];
            array[0] = 1;
            LeafA.CopyTo(array, 1);
            LeafB.CopyTo(array, (int) (LeafA.Length + 1));
            Tiger tiger = new Tiger();
            tiger.Initialize();
            return tiger.ComputeHash(array);
        }

        private byte[] LeafHash(byte[] Raw_Data)
        {
            byte[] array = new byte[Raw_Data.Length + 1];
            array[0] = 0;
            Raw_Data.CopyTo(array, 1);
            Tiger tiger = new Tiger();
            tiger.Initialize();
            return tiger.ComputeHash(array);
        }
    }
}

