using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.CGInterface
{
   public  interface IApp
    {
         List<NodeModel> Nodes { get; set; }
         NodeModel Node { get; set; }
         BlockModel CurrentBlock { get; set; }
    }
}
