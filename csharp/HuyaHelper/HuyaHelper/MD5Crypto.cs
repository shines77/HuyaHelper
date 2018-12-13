using System;
using System.Text;
using System.Security.Cryptography;

namespace HuyaHelper
{
    class MD5Crypto
    {
        static public string encrypt16(string plaintext)
        {
            const int startIndex = 4;
            const int maxLength = startIndex + 8;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            StringBuilder sb = new StringBuilder();
            for (int i = startIndex; i < maxLength; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }

        static public string encrypt16(string plaintext, int startIndex)
        {
            // Adjust the startIndex range in [0, 8].
            if (startIndex < 0)
                startIndex = 0;
            if (startIndex > 8)
                startIndex = 8;
            int maxLength = startIndex + 8;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            StringBuilder sb = new StringBuilder();
            for (int i = startIndex; i < maxLength; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }

        static public string encrypt32(string plaintext)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }

        static public string encryptBase64(string plaintext)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
