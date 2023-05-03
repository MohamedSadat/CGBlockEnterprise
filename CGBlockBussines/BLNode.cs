using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockBussines
{
    public class BLNode
    {
        private readonly IApp app;
        BLBlock xblock;
        public BLNode(IApp app)
        {
            this.app = app;
            xblock=new BLBlock(app);
        }

        public bool Confirm(BlockModel block)
        {
            block.PrevHash = app.CurrentBlock.Hash;
            block.Hash = BLCrytography.HashAlgoStd($"{app.CurrentBlock.Hash}{app.Node.Name}{block.Volume}{block.BlockSize}{block.Blockid}{block.TimeStamp}{block.Fee}");
            return false;
        }
        public bool Post(LedgerTransModel ledgerTrans, string privatekey)
        {
            ledgerTrans.TransDateTime = DateTime.Now;
            ledgerTrans.TimeStamp = DateTime.Now.Ticks;
            ledgerTrans.Hash = BLCrytography.HashAlgoStd($"{privatekey}{xblock.GetLastRTrans().Hash}{ledgerTrans.TimeStamp}{ledgerTrans.Amount}{ledgerTrans.Sender}{ledgerTrans.Reciver}");
            app.CurrentBlock.Transactions.Add(ledgerTrans);
            return true;
        }
        public BlockModel GenerateBlock()
        {
            var block=new BlockModel
            {
                PrevHash = app.CurrentBlock.Hash,
                Blockid = app.CurrentBlock.Blockid + 1,
                Node = app.Node.Name,
                TimeStamp=DateTime.Now.Ticks,
                

            };

           
            app.CurrentBlock = block;

            return block;
        }
    }
}
