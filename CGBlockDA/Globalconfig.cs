using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockDA
{
    public class GlobalConfig
    {
        public static string cn="";
        public static void StartupApp(IApp app) 
        {
            cn = "Password=Lotus6488;Persist Security Info=True;User ID=sa;Initial Catalog=CGNode;Data Source=kitchino.ddns.net";
            CNode x = new CNode();
            app.Node = x.GetNode();
            app.Node.Peers = x.GetPeers();
      
            app.Node.BlockChain = x.GetBlockChain();
            
           app.Node.LastBlock= x.GetBlock(app.Node.BlockChain.Count-1);
            if(app.Node.LastBlock.Transactions.Count<10)
            {
                app.Node.CurrentBlock = app.Node.LastBlock;
            }
        }
        public static void StartupWallet(WalletModel wallet)
        {
            cn = "Password=Lotus6488;Persist Security Info=True;User ID=sa;Initial Catalog=CGNode;Data Source=kitchino.ddns.net";
            CNode x = new CNode();
            wallet.Peers = x.GetPeers();
      
        }
    }
}
