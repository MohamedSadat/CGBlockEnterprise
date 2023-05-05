using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.Models
{
    public class BlockChainModel
    {
        public BlockChainModel()
        {
            
        }
        public BlockModel BlockHashModel { get; set; } = new BlockModel();
        public string BlockHash { get; set; }
        public BlockModel PrevBlockHashModel { get; set; }=new BlockModel();
        public string PrevBlockHash { get; set; }
        public double BlockHeight { get; set; }
  
    }
}
