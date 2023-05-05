using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockBussines
{
    public class BLCrytography
    {
        public static bool ValidateHashStd(string hash, string source)
        {
            var newhash = HashAlgoStd(source);
            if (newhash == hash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
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
        public static string HashTransaction(LedgerTransModel trans)
        {
            var hash = HashAlgoStd($"{trans.TransId}{trans.Sender}{trans.Reciver}{trans.Amount}{trans.TransDate}{trans.BlockHash}");
            return hash;
        }
       public static string GeneratePRivateKey()
        {
            // Generate a new private key
            byte[] privateKeyBytes = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(privateKeyBytes);
            }
            string privateKeyHex = BitConverter.ToString(privateKeyBytes).Replace("-", "");
            return privateKeyHex;

        }
    }
}
