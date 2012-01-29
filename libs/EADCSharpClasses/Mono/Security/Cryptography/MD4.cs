namespace Mono.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    public abstract class MD4 : HashAlgorithm
    {
        protected MD4()
        {
            base.HashSizeValue = 0x80;
        }

        public static MD4 Create()
        {
            return Create("MD4");
        }

        public static MD4 Create(string hashName)
        {
            object obj2 = CryptoConfig.CreateFromName(hashName);
            if (obj2 == null)
            {
                obj2 = new MD4Managed();
            }
            return (MD4) obj2;
        }
    }
}

