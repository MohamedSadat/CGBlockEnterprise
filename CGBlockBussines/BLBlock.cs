using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockBussines
{
    internal class BLBlock
    {
        public BlockModel Block { get; set; }
        public BLBlock(IApp app)
        {
         
        }
        public bool Validate(BlockModel block)
        {
            return false;
        }
        public bool Confirm(BlockModel block)
        {
            return false;
        }
        public bool Post(BlockModel block)
        {
            return false;
        }
        public LedgerTransModel GetLastRTrans()
        {
            return new LedgerTransModel();
        }

    }
}
