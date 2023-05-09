using CGBlockInfra.CGTypes;
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
        public string Receiver { get; set; } = "";
        public double Amount { get; set; } = 0;
        public double Fee { get; set; } = 0;
        public double Reward { get; set; } = 0;

        public string TransId { get; set; } = "";
        public string TransHash { get; set; } = "";
        public string Digest { get; set; } = "";

        public string Note { get; set; } = "";
        public int BlockHeight { get; set; } = 0;
        public string PublicKey { get; set; } = "";
        public TTransState TransStatus { get; set; } = TTransState.Created;
      public  TTransType TransType { get; set; } = TTransType.Normal;
        public List<UTXOModel> Inputs { get; set; } = new List<UTXOModel>();
        public List<UTXOModel> Outputs { get; set; } = new List<UTXOModel>();

        public string ErrorMSG { get; set; } = "";
    }
}
