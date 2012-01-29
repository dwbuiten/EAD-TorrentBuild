namespace EAD.Conversion
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Base32
    {
        private static char[] Base32Chars = new char[] { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '2', '3', '4', '5', '6', '7'
         };

        private Base32()
        {
        }

        public static byte[] FromBase32String(string s)
        {
            throw new NotImplementedException("Not implemented yet. Not required for Nap.");
        }

        public static string GetBase32Hash(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            SHA1 sha = new SHA1CryptoServiceProvider();
            return ToBase32String(sha.ComputeHash(bytes));
        }

        public static string ToBase32String(byte[] inArray)
        {
            if (inArray == null)
            {
                return null;
            }
            int length = inArray.Length;
            int num2 = length / 5;
            int num3 = length - (5 * num2);
            StringBuilder builder = new StringBuilder();
            int num4 = 0;
            for (int i = 0; i < num2; i++)
            {
                byte num6 = inArray[num4++];
                byte num7 = inArray[num4++];
                byte num8 = inArray[num4++];
                byte num9 = inArray[num4++];
                byte num10 = inArray[num4++];
                builder.Append(Base32Chars[num6 >> 3]);
                builder.Append(Base32Chars[((num6 << 2) & 0x1f) | (num7 >> 6)]);
                builder.Append(Base32Chars[(num7 >> 1) & 0x1f]);
                builder.Append(Base32Chars[((num7 << 4) & 0x1f) | (num8 >> 4)]);
                builder.Append(Base32Chars[((num8 << 1) & 0x1f) | (num9 >> 7)]);
                builder.Append(Base32Chars[(num9 >> 2) & 0x1f]);
                builder.Append(Base32Chars[((num9 << 3) & 0x1f) | (num10 >> 5)]);
                builder.Append(Base32Chars[num10 & 0x1f]);
            }
            if (num3 > 0)
            {
                byte num12;
                byte num13;
                byte num11 = inArray[num4++];
                builder.Append(Base32Chars[num11 >> 3]);
                switch (num3)
                {
                    case 1:
                        builder.Append(Base32Chars[(num11 << 2) & 0x1f]);
                        break;

                    case 2:
                        num12 = inArray[num4++];
                        builder.Append(Base32Chars[((num11 << 2) & 0x1f) | (num12 >> 6)]);
                        builder.Append(Base32Chars[(num12 >> 1) & 0x1f]);
                        builder.Append(Base32Chars[(num12 << 4) & 0x1f]);
                        break;

                    case 3:
                        num12 = inArray[num4++];
                        num13 = inArray[num4++];
                        builder.Append(Base32Chars[((num11 << 2) & 0x1f) | (num12 >> 6)]);
                        builder.Append(Base32Chars[(num12 >> 1) & 0x1f]);
                        builder.Append(Base32Chars[((num12 << 4) & 0x1f) | (num13 >> 4)]);
                        builder.Append(Base32Chars[(num13 << 1) & 0x1f]);
                        break;

                    case 4:
                    {
                        num12 = inArray[num4++];
                        num13 = inArray[num4++];
                        byte num14 = inArray[num4++];
                        builder.Append(Base32Chars[((num11 << 2) & 0x1f) | (num12 >> 6)]);
                        builder.Append(Base32Chars[(num12 >> 1) & 0x1f]);
                        builder.Append(Base32Chars[((num12 << 4) & 0x1f) | (num13 >> 4)]);
                        builder.Append(Base32Chars[((num13 << 1) & 0x1f) | (num14 >> 7)]);
                        builder.Append(Base32Chars[(num14 >> 2) & 0x1f]);
                        builder.Append(Base32Chars[(num14 << 3) & 0x1f]);
                        break;
                    }
                }
            }
            return builder.ToString();
        }
    }
}

