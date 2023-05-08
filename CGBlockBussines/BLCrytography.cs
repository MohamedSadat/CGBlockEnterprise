using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static bool ValidateBlockMerkle(BlockModel block) 
        {
            var curhash = block.MerkleRoot;
            var correcthash = CalculateMerkleRoot(block);
            if(curhash==correcthash)
            {
                return true;
            }
            else
            { return false; }
        }
        public static string CalculateMerkleRoot(BlockModel block)
        {
            string hash = "";
            foreach(var trans in block.Transactions) 
            {
                hash = hash + trans.TransHash;
            }
            block.MerkleRoot = HashAlgoStd(hash);
            return block.MerkleRoot;
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
        public static string MineBlock(BlockModel block)
        {
            var hash = HashAlgoStd($"{block.PrevBlockHash}{block.TimeStamp}{block.BlockHeight}{block.Nonce}");
            while(hash.StartsWith("000") == false)
            {
                block.Nonce++;
                hash = HashAlgoStd($"{block.PrevBlockHash}{block.TimeStamp}{block.BlockHeight}{block.Nonce}");
            }
          //  block.BlockHash = hash;
            return hash;

        

        }
        public static string HashTransaction(LedgerTransModel trans)
        {
            trans.TransId = GeneratePrivateKey();
            var hash = HashAlgoStd($"{trans.TransId}{trans.Sender}{trans.Receiver}{trans.Amount}{trans.TransDate}{trans.Note}{trans.NodeAddress}{trans.BlockHash}");
          //  trans.TransHash = hash;
            return hash;
        }
       public static string GeneratePrivateKey()
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
