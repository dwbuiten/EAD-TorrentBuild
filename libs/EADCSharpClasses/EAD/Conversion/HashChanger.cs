namespace EAD.Conversion
{
    using System;
    using System.Text;

    public class HashChanger
    {
        private string base32value;
        private byte[] bytehashvalue;
        private string hexhashvalue;
        private string rawhashvalue;

        public string base32
        {
            get
            {
                return this.base32value;
            }
        }

        public byte[] bytehash
        {
            get
            {
                return this.bytehashvalue;
            }
            set
            {
                this.bytehashvalue = value;
                this.hexhashvalue = "";
                this.rawhashvalue = "";
                StringBuilder builder = new StringBuilder();
                int index = 0;
                foreach (byte num2 in this.bytehashvalue)
                {
                    this.rawhashvalue = this.rawhashvalue + Encoding.Default.GetString(this.bytehashvalue, index, 1);
                    builder.AppendFormat("{0:x2}", num2);
                    index++;
                }
                this.hexhashvalue = builder.ToString();
                this.base32value = Base32.ToBase32String(this.bytehashvalue);
            }
        }

        public string hexhash
        {
            get
            {
                return this.hexhashvalue;
            }
            set
            {
                int num;
                this.hexhashvalue = value;
                this.bytehashvalue = HexEncoding.GetBytes(this.hexhashvalue, out num);
                this.rawhashvalue = Encoding.Default.GetString(this.bytehashvalue);
                this.base32value = Base32.ToBase32String(this.bytehashvalue);
            }
        }

        public string rawhash
        {
            get
            {
                return this.rawhashvalue;
            }
            set
            {
                this.rawhashvalue = value;
                this.bytehashvalue = Encoding.Default.GetBytes(value);
                StringBuilder builder = new StringBuilder();
                foreach (byte num in this.bytehashvalue)
                {
                    builder.AppendFormat("{0:x2}", num);
                }
                this.hexhashvalue = builder.ToString();
                this.base32value = Base32.ToBase32String(this.bytehashvalue);
            }
        }
    }
}

