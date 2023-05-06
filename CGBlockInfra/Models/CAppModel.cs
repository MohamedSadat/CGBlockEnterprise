using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.Models
{
    public class CAppModel : IApp
    {
    
        public NodeModel Node { get; set; } = new NodeModel();
   
    }
}
