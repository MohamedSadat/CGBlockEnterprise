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
        public long TimeStamp { get; set; } = DateTime.Now.Ticks;
        public DateTime TransDateTime { get; set; } = DateTime.Now;
        public string Sender { get; set; } = "";
        public string Reciver { get; set; } = "";
        public double Amount { get; set; } = 0.02f;
        public double Fee { get; set; } = 0.02f;
        public string TrId { get; set; } = "";
        public int BlockId { get; set; } = 0;
        public int TransStatus { get; set; } = 0;
        public int Confirmations { get; set; } = 0;
        public string Hash { get; set; } = "";
        public string Note { get; set; } = "";

    }
}
