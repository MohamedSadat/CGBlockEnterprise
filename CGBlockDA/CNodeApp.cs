using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockDA
{
    public class CNodeApp : INode
    {
        CNode xnode = new CNode();
        CLedgerTransCMD xledger = new CLedgerTransCMD();
        CUTXOExplorer xutxo= new CUTXOExplorer();
        public CNodeApp() 
        {

        }

        public double GetBalance(string publickey)
        {
            var address = DAHash.HashAlgoStd(publickey);
           return CUTXOExplorer.GetBalance(address);
        }

        public List<UTXOModel> GetUnpent(string publickey)
        {
            var address = DAHash.HashAlgoStd(publickey);
      return xutxo.GetUnpent(address);
        }

        public bool SaveBlock(BlockModel block)
        {
         return xnode.SaveBlock(block);
        }

        public bool SaveGenisusBlock(BlockModel block)
        {
          return  xnode.SaveGenisusBlock(block); 
            ;
        }

        public bool SaveLedgerTrans(LedgerTransModel transtable)
        {
         return xledger.SaveLedgerTrans(transtable);
        }
    }
}
