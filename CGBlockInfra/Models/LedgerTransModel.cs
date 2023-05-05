using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.Models
{
    public class LedgerTransModel
    {
        public LedgerTransModel()
        {
            
        }
        public BlockModel BlockIdModel { get; set; }
        public string BlockHash { get; set; } = "";
        public string NodeAddress { get; set; } = "";

        public long TimeStamp { get; set; } = DateTime.Now.Ticks;
        public DateTime TransDate { get; set; } = DateTime.Now;
        public string Sender { get; set; } = "";
        public string Reciver { get; set; } = "";
        public double Amount { get; set; } = 0.02f;
        public double Fee { get; set; } = 0.02f;
        public long TransId { get; set; } = 0;
        public int TransStatus { get; set; } = 0;
        public string TransHash { get; set; } = "";
        public string Note { get; set; } = "";
        public List<UTXOModel> Inputs { get; set; } = new List<UTXOModel>();
        public List<UTXOModel> Outputs { get; set; } = new List<UTXOModel>();


    }
}
