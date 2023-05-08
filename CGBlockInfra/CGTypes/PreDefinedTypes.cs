using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.CGTypes
{
    public enum TBlockState { NotSet, Saved, Confirmed };
    public enum TTransState { Created, Validated, Pending,Completed };
    public enum TTransType { CoinBase, Normal };

}
