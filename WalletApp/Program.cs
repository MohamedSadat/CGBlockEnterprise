using CGBlockBussines;
using CGBlockDA;
using CGBlockInfra.Models;
using Org.BouncyCastle.Utilities.Encoders;
using System.Security.Cryptography;
using System.Text;

namespace WalletApp
{
    public class Program
    {
        static WalletModel wallet=new WalletModel();
      static  CAppModel app=new CAppModel();
       static string _privateKey= "58AE8D527DF86DEB04F2C256ACF185E81DE86C9A286D608782F214CFC95D6FB8";
        static void Main(string[] args)
        {
          
            Console.WriteLine("Private Key: " + _privateKey + Environment.NewLine);

            LedgerTransModel trans = new LedgerTransModel { Sender = "c7d67c3ad12097f3a86a8c0b23380c511ee5322558dc49553944559352e2f325",
                Receiver = "9766d4ffaa2e599dddc66ff936948fc7fad3961227dfebda0e21f6e3ae012400",
                PublicKey= "bf020af2dadec840cc6e979924be2c75e1a4c3567c93185ed5a0f7beff8df5a4",
                Amount = 10 };
            Console.WriteLine("Public Key: " + trans.PublicKey + Environment.NewLine);

            SignTransaction(trans);


            Console.WriteLine("Public Key: " + trans.PublicKey + Environment.NewLine);

            Console.WriteLine("Digest: " + Environment.NewLine + trans.Digest + Environment.NewLine);
            var dec = Validate(trans);
            Console.WriteLine("Decrypted Text: " + Environment.NewLine + dec.ToString() + Environment.NewLine);

            Console.Read();
        }
        public static void SignTransaction(LedgerTransModel trans)
        {
            RSACryptoServiceProvider rsa = new();
            _privateKey = rsa.ToXmlString(false);
            trans.PublicKey = rsa.ToXmlString(true);

            var hash=BLCrytography.HashTransaction(trans);
            trans.TransHash = hash;
            Console.WriteLine($"trans hash: {hash}");
            rsa.FromXmlString(_privateKey);

            byte[] dataToEncrypt = Encoding.ASCII.GetBytes(hash);
            byte[] encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();

            var r= Convert.ToBase64String(encryptedByteArray);
            trans.Digest = r;
         
        }


        public static bool Validate(LedgerTransModel trans)
        {
            try
            {


                RSACryptoServiceProvider rsa = new();
                rsa.FromXmlString(trans.PublicKey);

                byte[] dataByte = Convert.FromBase64String(trans.Digest);
                byte[] decryptedByte = rsa.Decrypt(dataByte, false);

                var r = Encoding.UTF8.GetString(decryptedByte);
                if (r == trans.TransHash)
                {
                    Console.WriteLine("Valid");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        
        }

    
        public static void Send()
        {

            GlobalConfig.StartupWallet(wallet);
            GlobalConfig.StartupApp(app);
            BLWallet bLWallet = new BLWallet();
            bLWallet.Wallet = wallet;
            wallet.Peers[0] = app.Node;
            wallet.PrivateKey = "58AE8D527DF86DEB04F2C256ACF185E81DE86C9A286D608782F214CFC95D6FB8";
            wallet.PublicKey = "bf020af2dadec840cc6e979924be2c75e1a4c3567c93185ed5a0f7beff8df5a4";
            wallet.Address = "c7d67c3ad12097f3a86a8c0b23380c511ee5322558dc49553944559352e2f325";

            var trans = new LedgerTransModel
            {
                Amount = 2,
                Sender = bLWallet.Wallet.Address,
                Receiver = "9766d4ffaa2e599dddc66ff936948fc7fad3961227dfebda0e21f6e3ae012400",
                Note = "send again",
                Fee = 0,
                PublicKey = wallet.PublicKey,
                TransType = CGBlockInfra.CGTypes.TTransType.Normal
            };

            foreach (var node in wallet.Peers)
            {
                Console.WriteLine(node.NodeName + ": " + node.Address);
            }

            bLWallet.Sign(trans, bLWallet.Wallet.PrivateKey);
            // bLWallet.Send(trans, bLWallet.Wallet.PublicKey);

            Console.WriteLine(trans.ErrorMSG);

        }
    }
}