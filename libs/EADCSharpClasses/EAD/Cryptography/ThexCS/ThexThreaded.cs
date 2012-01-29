namespace EAD.Cryptography.ThexCS
{
    using System;
    using System.IO;
    using System.Threading;

    public class ThexThreaded
    {
        private const int DataBlockSize = 0x200000;
        private string Filename;
        private FileBlock[] FileParts = new FileBlock[2];
        private FileStream FilePtr;
        private const byte InternalHash = 1;
        private int LeafCount;
        private const byte LeafHash = 0;
        private const int LeafSize = 0x400;
        public int LevelCount;
        private const int ThreadCount = 2;
        private Thread[] ThreadsList = new Thread[2];
        public byte[][][] TTH;

        private void CompressTree()
        {
            int index = 0;
            while ((index + 1) < this.LevelCount)
            {
                int num4 = 0;
                int num = (this.LeafCount / 2) + (this.LeafCount % 2);
                this.TTH[index + 1] = new byte[num][];
                for (int i = 1; i < this.LeafCount; i += 2)
                {
                    this.ProcessInternalLeaf(index + 1, num4++, this.TTH[index][i - 1], this.TTH[index][i]);
                }
                if (num4 < num)
                {
                    this.TTH[index + 1][num4] = this.TTH[index][this.LeafCount - 1];
                }
                index++;
                this.LeafCount = num;
            }
        }

        private void Dispose()
        {
            this.TTH = null;
            this.ThreadsList = null;
            this.FileParts = null;
            GC.Collect();
        }

        private void GetTTH(string Filename)
        {
            this.Filename = Filename;
            this.OpenFile();
            this.Initialize();
            this.SplitFile();
            Console.WriteLine("starting to get TTH: " + DateTime.Now.ToString());
            this.StartThreads();
            Console.WriteLine("finished to get TTH: " + DateTime.Now.ToString());
            GC.Collect();
            this.CompressTree();
            if (this.FilePtr != null)
            {
                this.FilePtr.Close();
            }
        }

        public byte[][][] GetTTH_Tree(string Filename)
        {
            this.GetTTH(Filename);
            return this.TTH;
        }

        public byte[] GetTTH_Value(string Filename)
        {
            this.GetTTH(Filename);
            return this.TTH[this.LevelCount - 1][0];
        }

        private void Initialize()
        {
            int num = 1;
            this.LevelCount = 1;
            this.LeafCount = (int) (this.FilePtr.Length / 0x400L);
            if ((this.FilePtr.Length % 0x400L) > 0L)
            {
                this.LeafCount++;
            }
            while (num < this.LeafCount)
            {
                num *= 2;
                this.LevelCount++;
            }
            this.TTH = new byte[this.LevelCount][][];
            this.TTH[0] = new byte[this.LeafCount][];
        }

        private void OpenFile()
        {
            if (!File.Exists(this.Filename))
            {
                throw new Exception("file doesn't exists!");
            }
            this.FilePtr = new FileStream(this.Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        private void ProcessInternalLeaf(int Level, int Index, byte[] LeafA, byte[] LeafB)
        {
            Tiger tiger = new Tiger();
            byte[] dst = new byte[(LeafA.Length + LeafB.Length) + 1];
            dst[0] = 1;
            Buffer.BlockCopy(LeafA, 0, dst, 1, LeafA.Length);
            Buffer.BlockCopy(LeafB, 0, dst, LeafA.Length + 1, LeafA.Length);
            tiger.Initialize();
            this.TTH[Level][Index] = tiger.ComputeHash(dst);
        }

        private void ProcessLeafs()
        {
            byte[] buffer;
            FileStream stream = new FileStream(this.Filename, FileMode.Open, FileAccess.Read);
            FileBlock block = this.FileParts[Convert.ToInt16(Thread.CurrentThread.Name)];
            Tiger tiger = new Tiger();
            byte[] dst = new byte[0x401];
            stream.Position = block.Start;
            while (stream.Position < block.End)
            {
                uint num = ((uint) stream.Position) / 0x400;
                if ((block.End - stream.Position) < 0x200000L)
                {
                    buffer = new byte[block.End - stream.Position];
                }
                else
                {
                    buffer = new byte[0x200000];
                }
                stream.Read(buffer, 0, buffer.Length);
                int num2 = buffer.Length / 0x400;
                int num3 = 0;
                while (num3 < num2)
                {
                    Buffer.BlockCopy(buffer, num3 * 0x400, dst, 1, 0x400);
                    tiger.Initialize();
                    this.TTH[0][num++] = tiger.ComputeHash(dst);
                    num3++;
                }
                if ((num3 * 0x400) < buffer.Length)
                {
                    dst = new byte[(buffer.Length - (num2 * 0x400)) + 1];
                    dst[0] = 0;
                    Buffer.BlockCopy(buffer, num2 * 0x400, dst, 1, dst.Length - 1);
                    tiger.Initialize();
                    this.TTH[0][num++] = tiger.ComputeHash(dst);
                    dst = new byte[0x401];
                    dst[0] = 0;
                }
            }
            buffer = null;
            dst = null;
        }

        private void SplitFile()
        {
            long num = this.LeafCount / 2;
            if (this.FilePtr.Length > 0x100000L)
            {
                for (int i = 0; i < 2; i++)
                {
                    this.FileParts[i] = new FileBlock((num * 0x400L) * i, (num * 0x400L) * (i + 1));
                }
            }
            this.FileParts[1].End = this.FilePtr.Length;
        }

        private void StartThreads()
        {
            for (int i = 0; i < 2; i++)
            {
                this.ThreadsList[i] = new Thread(new ThreadStart(this.ProcessLeafs));
                this.ThreadsList[i].IsBackground = true;
                this.ThreadsList[i].Name = i.ToString();
                this.ThreadsList[i].Start();
            }
            bool flag = false;
            do
            {
                Thread.Sleep(0x3e8);
                flag = false;
                for (int j = 0; j < 2; j++)
                {
                    if (this.ThreadsList[j].IsAlive)
                    {
                        flag = true;
                    }
                }
            }
            while (flag);
        }

        private void StopThreads()
        {
            for (int i = 0; i < 2; i++)
            {
                if ((this.ThreadsList[i] != null) && this.ThreadsList[i].IsAlive)
                {
                    this.ThreadsList[i].Abort();
                }
            }
        }
    }
}

