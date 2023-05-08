using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.WebView
{
    public class AccountVM
    {
        public AccountVM()
        {
            
        }
        public string Address { get; set; } = "";
        public List<LedgerTransModel> Transactions { get; set; }=new List<LedgerTransModel>();
    }
}
