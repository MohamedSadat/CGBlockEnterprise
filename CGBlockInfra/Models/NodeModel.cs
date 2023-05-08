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
        public string NodeName { get; set; }    
        public string Address { get; set; } = "";
        public string PublicKey { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public List<BlockChainModel> BlockChain { get; set; }=new List<BlockChainModel>();
        public List<NodeModel> Peers { get; set; } = new List<NodeModel>();

        public BlockModel CurrentBlock { get; set; } = new BlockModel();
       public BlockModel LastBlock { get; set; } = new BlockModel();

        public Stack<LedgerTransModel> MemPool { get; set; }=new Stack<LedgerTransModel>();

    }
}
