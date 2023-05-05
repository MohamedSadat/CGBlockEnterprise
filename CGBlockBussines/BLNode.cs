using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGBlockDA;
namespace CGBlockBussines
{
    public class BLNode
    {
        CNode xnode = new CNode();
        CUTXOExplorer xexplorer = new CUTXOExplorer();
        private readonly IApp app;
        BLBlock xblock;
        public NodeModel Node { get; set; } = new NodeModel();
        CLedgerTransCMD xledger = new CLedgerTransCMD();
        public BLNode(IApp app)
        {
            this.app = app;
            xblock=new BLBlock(app);
        }

        public bool Confirm(BlockModel block)
        {
            block.PrevHash = app.CurrentBlock.BlockHash;
            block.BlockHash = BLCrytography.HashAlgoStd($"{app.CurrentBlock.BlockHash}{app.Node.Name}{block.Volume}{block.BlockSize}{block.Blockid}{block.TimeStamp}{block.Fee}");
            return false;
        }
        public bool Post(LedgerTransModel ledgerTrans, string privatekey)
        {
            ledgerTrans.TransDate = DateTime.Now;
            ledgerTrans.TimeStamp = DateTime.Now.Ticks;
            ledgerTrans.TransHash = BLCrytography.HashAlgoStd($"{privatekey}{xblock.GetLastRTrans().TransHash}{ledgerTrans.TimeStamp}{ledgerTrans.Amount}{ledgerTrans.Sender}{ledgerTrans.Reciver}");
            app.CurrentBlock.Transactions.Add(ledgerTrans);
            return true;
        }
  
        public void AddNode(NodeModel node)
        {
            Node.Peers.Add(node);
        }
        public void BroadCast(BlockModel block)
        {
            foreach(var peer in Node.Peers)
            {

            }
        }
        public BlockModel GetLastBlock()
        {
            return new BlockModel();
        }
        public bool ValidateTransaction(LedgerTransModel ledgerTrans, string privatekey)
        {
            var publickey = BLCrytography.HashAlgoStd(privatekey);
            //Search UTXO to validate balance

            return true;
        }
        public void GenerateGenisusBlock()
        {
            var block = new BlockModel
            {
                PrevHash = "0000000000000000000000000000000000000000000000000000000000000000",
                BlockHash = "0000000000000000000000000000000000000000000000000000000000000000",
                TimeStamp = DateTime.Now.Ticks,
                BlockHeight = 0
            };
            var trans = new LedgerTransModel
            {
                BlockHash = block.BlockHash,
                TimeStamp = DateTime.Now.Ticks,
                TransDate = DateTime.Now,
                Reciver = Node.Address,
                Amount = 50,
                NodeAddress = Node.Address,
                Note = "Genisus block"
            };
            block.Transactions.Add(trans);
            Node.BlockChain.Add(new BlockChainModel
            {
                BlockHash = block.BlockHash,
                PrevBlockHash = block.PrevHash,
                BlockHeight = block.BlockHeight,
                BlockHashModel=block
            });
            var utxo = new UTXOModel
            {
                Address = Node.Address,
                PublikKey = Node.PublicKey,
                Id = 0,
                TransId = trans.TransId,
                OutputIndex = 0
               
            };
            trans.Outputs.Add(utxo);
            xnode.SaveGenisusBlock(block);
        }
        public void GenerateBlock()
        {
            var block = new BlockModel
            {
                PrevHash = app.CurrentBlock.BlockHash,
                TimeStamp = DateTime.Now.Ticks,
                BlockHeight = 0,
                Node=app.Node.Address
            };
            block.BlockHash = BLCrytography.HashAlgoStd($"{block.PrevHash}{block.TimeStamp}{block.BlockHeight}");

            Node.BlockChain.Add(new BlockChainModel
            {
                BlockHash = block.BlockHash,
                PrevBlockHash = block.PrevHash,
                BlockHeight = block.BlockHeight,
                BlockHashModel = block
            });

        }
        public void ReceiveTransaction(LedgerTransModel trans,string publickey)
        {

            var address = BLCrytography.HashAlgoStd(publickey);
            trans.Sender = address;
            trans.TransHash = BLCrytography.HashTransaction(trans);
            trans.Inputs = xexplorer.GetInputs(publickey);
            if(trans.Inputs.Sum(x => x.Amount) < trans.Amount)
            {
                //not enough balance
            }

            //Validate Key
            //Retrieve un spent UTXO
            //Generate new utxo
            var lastblock = GetLastBlock();
                lastblock.Transactions.Add(trans);

        }
        public void GenerateOutput(LedgerTransModel trans)
        {
            foreach(var vin in trans.Inputs)
            {
                var output = vin.Amount- trans.Amount ;
                if (output > 0)
                {
                    vin.Spent = 1;
                    var vout = new UTXOModel
                    {
                        Amount = output,
                        TransId = trans.TransId,
                        Address = trans.Sender,
                        OutputIndex = 0
                    };
                    trans.Outputs.Add(vout);
                }
                if(output==0)
                {
                    vin.Spent = 1;
                }
                else if(output<0)
                {

                }
            }
        }
    }
}
