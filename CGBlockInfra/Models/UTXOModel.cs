using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.Models
{
    public class UTXOModel
    {
        public UTXOModel()
        {
            
        }
        public int Id { get; set; }
        public long TransId { get; set; }
        public int OutputIndex { get; set; } = 0;
        public string Address { get; set; }
        public string PublikKey { get; set; }
        public int Spent { get; set; } = 0;
        public double Amount { get; set; } = 0;

    }
}
