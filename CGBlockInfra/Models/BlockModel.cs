using CGBlockInfra.CGTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.Models
{
    public class BlockModel
    {
        public BlockModel()
        {
        }
        public List<LedgerTransModel> Transactions { get; set; } = new List<LedgerTransModel>();
        public long TimeStamp { get; set; } = DateTime.Now.Ticks;
        public DateTime BlockDate { get; set; } = DateTime.Now;
        public string   PrevBlockHash { get; set; }
        public string BlockHash { get; set; }
        public double Volume { get; set; } = 0;
        public int BlockHeight { get; set; } = 0;
        public double Fee { get; set; } = 0;
        public double Reward { get; set; } = 0;
    
        public string NodeAddress { get; set; } = "";
        public int BlockSize { get; set; } = 0;
        public TBlockState BlockState { get; set; } = TBlockState.NotSet;
    }
}
