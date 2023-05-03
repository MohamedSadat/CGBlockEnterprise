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
        public string   PrevHash { get; set; }
        public string Hash { get; set; }
        public int Blockid { get; set; }
        public double Volume { get; set; } = 0;
        public int Blocksize { get; set; } = 0;
        public double Fee { get; set; } = 0;
        public double Reward { get; set; } = 0;
        public string Node { get; set; } = "";
        public int Confirmations { get; set; } = 0;
        public int BlockSize { get; set; } = 0;
    }
}
