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
        public BLWallet()
        {
          //  xnode=new BLNode()
        }
        public void Sign(LedgerTransModel trans, string privatekey )
        {
            var publickey = BLCrytography.HashAlgoStd(privatekey);
            trans.Sender = publickey;
            Send(trans, publickey);
        }
        public void Send(LedgerTransModel trans, string publickey )
        {
            xnode.ReceiveTransaction(trans, publickey); 
        }
    }
}
