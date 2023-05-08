using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockBussines
{
    public class BLConsensus
    {
        public BLConsensus() { }
        public bool ValidateBlock (BlockModel block)
        {
            //Validate Block Hash
            //Validate Block Marble
            if(block.BlockHash.StartsWith("000")==false)
            {
                return false;
            }
           if(BLCrytography.ValidateBlockMerkle(block)==false) 
            {
                //Reject the block
                return false;
            }
            foreach(var trans in block.Transactions)
            {
              if(  ValidateTransaction (trans)==false);
                return false;
            }
            return true;
        }
        public bool ValidateTransaction(LedgerTransModel trans)
        {
            var currenthash = trans.TransHash;
            var correcthash = BLCrytography.HashTransaction(trans);
            if(currenthash == correcthash)
            return true;
            else
                return false;
        }
        public bool ValidateNode(NodeModel node)
        {
           
            return true;
        }

    }
}
