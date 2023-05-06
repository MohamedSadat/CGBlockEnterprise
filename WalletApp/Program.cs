using CGBlockBussines;
using CGBlockDA;
using CGBlockInfra.Models;

namespace WalletApp
{
    public class Program
    {
        static WalletModel wallet=new WalletModel();
      static  CAppModel app=new CAppModel();
        static void Main(string[] args)
        {
            GlobalConfig.StartupWallet(wallet);
            GlobalConfig.StartupApp(app);
            BLWallet bLWallet = new BLWallet();
            bLWallet.Wallet=wallet;
            wallet.Peers[0] = app.Node;
            wallet.PrivateKey = "58AE8D527DF86DEB04F2C256ACF185E81DE86C9A286D608782F214CFC95D6FB8";
            wallet.PublicKey = "bf020af2dadec840cc6e979924be2c75e1a4c3567c93185ed5a0f7beff8df5a4";
            wallet.Address = "c7d67c3ad12097f3a86a8c0b23380c511ee5322558dc49553944559352e2f325";

            var trans = new LedgerTransModel {Amount=15,
            Sender=bLWallet.Wallet.Address,
            Receiver= "9766d4ffaa2e599dddc66ff936948fc7fad3961227dfebda0e21f6e3ae012400",
            Note="send again",
            Fee=0,
            PublicKey=wallet.PublicKey,
            };

            foreach (var node in wallet.Peers)
            {
                Console.WriteLine(node.NodeName + ": " + node.Address);
            }

            bLWallet.Sign(trans, bLWallet.Wallet.PrivateKey);
            // bLWallet.Send(trans, bLWallet.Wallet.PublicKey);

            Console.WriteLine(trans.ErrorMSG);


            Console.WriteLine("Hello, World!");
            Console.Read();
        }
    }
}