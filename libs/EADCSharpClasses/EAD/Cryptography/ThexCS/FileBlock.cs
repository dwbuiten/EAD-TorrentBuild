namespace EAD.Cryptography.ThexCS
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct FileBlock
    {
        public long Start;
        public long End;
        public FileBlock(long Start, long End)
        {
            this.Start = Start;
            this.End = End;
        }
    }
}

