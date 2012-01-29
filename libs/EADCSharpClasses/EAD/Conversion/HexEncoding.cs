namespace EAD.Conversion
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public class HexEncoding
    {
        public static int GetByteCount(string hexString)
        {
            int num = 0;
            for (int i = 0; i < hexString.Length; i++)
            {
                char c = hexString[i];
                if (IsHexDigit(c))
                {
                    num++;
                }
            }
            if ((num % 2) != 0)
            {
                num--;
            }
            return (num / 2);
        }

        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string str = "";
            for (int i = 0; i < hexString.Length; i++)
            {
                char c = hexString[i];
                if (IsHexDigit(c))
                {
                    str = str + c;
                }
                else
                {
                    discarded++;
                }
            }
            if ((str.Length % 2) != 0)
            {
                discarded++;
                str = str.Substring(0, str.Length - 1);
            }
            int num2 = str.Length / 2;
            byte[] buffer = new byte[num2];
            int num3 = 0;
            for (int j = 0; j < buffer.Length; j++)
            {
                string hex = new string(new char[] { str[num3], str[num3 + 1] });
                buffer[j] = HexToByte(hex);
                num3 += 2;
            }
            return buffer;
        }

        private static byte HexToByte(string hex)
        {
            if ((hex.Length > 2) || (hex.Length <= 0))
            {
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            }
            return byte.Parse(hex, NumberStyles.HexNumber);
        }

        public static bool InHexFormat(string hexString)
        {
            foreach (char ch in hexString)
            {
                if (!IsHexDigit(ch))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsHexDigit(char c)
        {
            int num2 = Convert.ToInt32('A');
            int num3 = Convert.ToInt32('0');
            c = char.ToUpper(c);
            int num = Convert.ToInt32(c);
            return (((num >= num2) && (num < (num2 + 6))) || ((num >= num3) && (num < (num3 + 10))));
        }

        public static string ToString(byte[] bytes)
        {
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str = str + bytes[i].ToString("X2");
            }
            return str;
        }
    }
}

