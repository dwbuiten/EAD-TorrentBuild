namespace EAD.Conversion
{
    using System;

    public class PortToByte
    {
        public int iPort;
        private byte[] pTempCompact;

        public byte[] pCompact
        {
            get
            {
                this.pTempCompact[0] = (byte) ((this.iPort & 0xff00) >> 8);
                this.pTempCompact[1] = (byte) (this.iPort & 0xff);
                return this.pTempCompact;
            }
        }
    }
}

