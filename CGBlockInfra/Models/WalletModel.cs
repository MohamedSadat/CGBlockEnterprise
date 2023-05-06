using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.Models
{
    public class WalletModel
    {
        public WalletModel()
        {
            
        }
        public string Address { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public List<NodeModel> Peers { get; set; } = new List<NodeModel>();

    }
}
