using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockBussines
{
    public class BLWallet
    {
        BLNode xnode;
        public WalletModel Wallet { get; set; } = new WalletModel();
        public BLWallet()
        {
          //  xnode=new BLNode()
        }
        public void GeneratePrivateKey()
        {
         Wallet.PrivateKey=    BLCrytography.GeneratePrivateKey();
        }
        public void GetPublicKey ()
        {
           Wallet.PublicKey = BLCrytography.HashAlgoStd(Wallet.PrivateKey);
        }
        public void GetAddres()
        {
            Wallet.Address= BLCrytography.HashAlgoStd(Wallet.PublicKey);
        }

        public void Sign(LedgerTransModel trans, string privatekey )
        {
            var publickey = BLCrytography.HashAlgoStd(privatekey);
            trans.PublicKey = publickey;
            Send(trans);
        }
        public void Send(LedgerTransModel trans )
        {
            //Prepare inputs,outputs

            IApp app = new CAppModel();
            app.Node = Wallet.Peers[0];

            xnode = new BLNode(app);
            xnode.ReceiveTransaction(trans);
            //foreach(var node in Wallet.Peers)
            //{
               
            //    app.Node = node;
            //    BLNode x = new BLNode(app);
            //    x.ReceiveTransaction(trans);

            //}
        }
    }
}
