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
        public string Description { get; set; } = "";
        public string Type { get; set; }
        public int NodeStatus { get; set; } = 0;
        public string NodeHash { get; set; } = "";
        public string Address { get; set; } = "";
        public string AddressHash { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public List<BlockModel> Blocks { get; set; } = new List<BlockModel>();
    }
}
