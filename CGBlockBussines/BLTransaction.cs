using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;

namespace CGBlockBussines
{
    public class BLTransaction
    {
        public BLTransaction(IApp app)
        {
            
        }
        public bool Validate (LedgerTransModel ledgerTrans)
        {
            return true;
        }
        public bool Confirm(LedgerTransModel ledgerTrans)
        {
            return true;
        }
        public bool Post(LedgerTransModel ledgerTrans,string privatekey)
        {
            return true;
        }


    }
}
