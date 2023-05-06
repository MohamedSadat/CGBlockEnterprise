using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockInfra.CGInterface
{
    public interface INode
    {
        bool SaveGenisusBlock(BlockModel block);
        bool SaveBlock(BlockModel block);
        bool SaveLedgerTrans(LedgerTransModel transtable);
        List<UTXOModel> GetUnpent(string publickey);
        double GetBalance(string publickey);
    }
}
