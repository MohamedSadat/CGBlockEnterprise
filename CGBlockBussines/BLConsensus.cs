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
            return true;
        }
        public bool ValidateTransaction(LedgerTransModel trans)
        {
            return true;
        }
        public bool ValidateNode(NodeModel node)
        {
            foreach(var block in node.Blocks)
            {

                foreach (var tx in block.Transactions)
                {

                }
            }
            return true;
        }

    }
}
