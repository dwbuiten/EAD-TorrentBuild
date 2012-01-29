namespace EAD.Conversion
{
    using EAD.CSharp;
    using System;
    using System.Text;

    public class EADCoreHash : Constants
    {
        public bool HashConvert(ref byte[] hashbytes, ref string Binary, ref string hexadecimal, int source)
        {
            int num3;
            bool flag = false;
            StringBuilder builder = new StringBuilder();
            if (source == 1)
            {
                Binary = "";
                hexadecimal = "";
                foreach (byte num in hashbytes)
                {
                    Binary = Binary + num.ToString();
                    builder.AppendFormat("{0:x2}", num);
                }
                hexadecimal = builder.ToString();
                return true;
            }
            if (source == 2)
            {
                hexadecimal = "";
                hashbytes = Encoding.Default.GetBytes(Binary);
                foreach (byte num2 in hashbytes)
                {
                    builder.AppendFormat("{0:x2}", num2);
                }
                hexadecimal = builder.ToString();
                return true;
            }
            if (source != 3)
            {
                return flag;
            }
            Binary = "";
            hashbytes = HexEncoding.GetBytes(hexadecimal, out num3);
            foreach (byte num4 in hashbytes)
            {
                Binary = Binary + num4.ToString();
            }
            return true;
        }
    }
}

