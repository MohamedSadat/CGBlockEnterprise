using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockDA
{
    public static class DAHash
    {
        public static string HashAlgoStd(string password, int outputSize = 32)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                byte[] truncatedBytes = new byte[outputSize];
                Array.Copy(bytes, truncatedBytes, outputSize);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in truncatedBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
