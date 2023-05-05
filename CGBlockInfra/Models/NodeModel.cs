using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.Models
{
    public class NodeModel
    {
        public NodeModel()
        {
            
        }
        public string Name { get; set; }    
        public string Address { get; set; } = "";
        public string PublicKey { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public List<BlockChainModel> BlockChain { get; set; }=new List<BlockChainModel>();
        public List<NodeModel> Peers { get; set; } = new List<NodeModel>();
    }
}
