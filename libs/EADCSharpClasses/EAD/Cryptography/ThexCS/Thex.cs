namespace EAD.Cryptography.ThexCS
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;

    public class Thex
    {
        private const int Block_Size = 0x400;
        private FileStream FilePtr;
        private int Leaf_Count;
        private ArrayList LeafCollection;

        private byte[] ByteExtract(byte[] Raw_Data, int Data_Length)
        {
            byte[] buffer = new byte[Data_Length];
            for (int i = 0; i < Data_Length; i++)
            {
                buffer[i] = Raw_Data[i];
            }
            return buffer;
        }

        private HashHolder GetRootHash()
        {
            ArrayList list = new ArrayList();
            do
            {
                list = new ArrayList(this.LeafCollection);
                this.LeafCollection.Clear();
                while (list.Count > 1)
                {
                    byte[] hashValue = ((HashHolder) list[0]).HashValue;
                    byte[] leafB = ((HashHolder) list[1]).HashValue;
                    this.LeafCollection.Add(new HashHolder(this.IH(hashValue, leafB)));
                    list.RemoveAt(0);
                    list.RemoveAt(0);
                }
                if (list.Count > 0)
                {
                    this.LeafCollection.Add(list[0]);
                }
            }
            while (this.LeafCollection.Count > 1);
            return (HashHolder) this.LeafCollection[0];
        }

        public byte[] GetTTH(string Filename)
        {
            try
            {
                this.FilePtr = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (this.FilePtr.Length <= 0x400L)
                {
                    return this.SmallFile();
                }
                this.Leaf_Count = ((int) this.FilePtr.Length) / 0x400;
                if ((this.FilePtr.Length % 0x400L) > 0L)
                {
                    this.Leaf_Count++;
                }
                this.LoadLeafHash();
                return this.GetRootHash().HashValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private byte[] IH(byte[] LeafA, byte[] LeafB)
        {
            byte[] array = new byte[(LeafA.Length + LeafB.Length) + 1];
            array[0] = 1;
            LeafA.CopyTo(array, 1);
            LeafB.CopyTo(array, (int) (LeafA.Length + 1));
            Tiger tiger = new Tiger();
            tiger.Initialize();
            return tiger.ComputeHash(array);
        }

        private byte[] LH(byte[] Raw_Data)
        {
            byte[] array = new byte[Raw_Data.Length + 1];
            array[0] = 0;
            Raw_Data.CopyTo(array, 1);
            Tiger tiger = new Tiger();
            tiger.Initialize();
            return tiger.ComputeHash(array);
        }

        private void LoadLeafHash()
        {
            this.LeafCollection = new ArrayList();
            for (int i = 0; i < (this.Leaf_Count / 2); i++)
            {
                byte[] buffer = new byte[0x400];
                byte[] buffer2 = new byte[0x400];
                this.FilePtr.Read(buffer, 0, 0x400);
                int num2 = this.FilePtr.Read(buffer2, 0, 0x400);
                if (num2 < 0x400)
                {
                    buffer2 = this.ByteExtract(buffer2, num2);
                }
                buffer = this.LH(buffer);
                buffer2 = this.LH(buffer2);
                this.LeafCollection.Add(new HashHolder(this.IH(buffer, buffer2)));
            }
            if ((this.Leaf_Count % 2) != 0)
            {
                byte[] buffer3 = new byte[0x400];
                int num3 = this.FilePtr.Read(buffer3, 0, 0x400);
                if (num3 < 0x400)
                {
                    buffer3 = this.ByteExtract(buffer3, num3);
                }
                this.LeafCollection.Add(new HashHolder(this.LH(buffer3)));
            }
            this.FilePtr.Close();
        }

        private byte[] SmallFile()
        {
            new Tiger();
            byte[] buffer = new byte[0x400];
            int num = this.FilePtr.Read(buffer, 0, 0x400);
            this.FilePtr.Close();
            return this.LH(this.ByteExtract(buffer, num));
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HashHolder
        {
            public byte[] HashValue;
            public HashHolder(byte[] HashValue)
            {
                this.HashValue = HashValue;
            }
        }
    }
}

