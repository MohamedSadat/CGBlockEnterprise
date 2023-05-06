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
        BLUTXO xutxo;
        public BLNode(IApp app)
        {
            this.app = app;
            xblock=new BLBlock(app);
            xutxo=new BLUTXO(app);
        }

        public bool Confirm(BlockModel block)
        {
            block.PrevHash = app.CurrentBlock.BlockHash;
            block.BlockHash = BLCrytography.HashAlgoStd($"{app.CurrentBlock.BlockHash}{app.Node.Name}{block.Volume}{block.BlockSize}{block.Blockid}{block.TimeStamp}{block.Fee}");
            return false;
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

            //Generate Reward transaction
          block.Transactions.Add(  GenerateRewardTransaction(block)); 
           
            Node.BlockChain.Add(new BlockChainModel
            {
                BlockHash = block.BlockHash,
                PrevBlockHash = block.PrevHash,
                BlockHeight = block.BlockHeight,
                BlockHashModel = block
            });

        }
        public LedgerTransModel GenerateRewardTransaction(BlockModel block)
        {
            var trans = new LedgerTransModel
            {
                Amount = 50,
                BlockHash = block.BlockHash,
                Fee = 0,
                Note = "Reward",
                Reciver = block.Node,
                TransDate = DateTime.Now,
                TimeStamp = DateTime.Now.Ticks,
            };
            BLCrytography.HashTransaction(trans);
            var output = new UTXOModel
            {
                Address = block.Node,
                Amount = trans.Amount,
                OutputIndex = 0,
                PublikKey = "",
                Spent = 0,
                TransId = trans.TransId
            };
            trans.Outputs.Add(output);
            return trans;

        }
        public void ReceiveTransaction(LedgerTransModel trans,string publickey)
        {
            
            var address = BLCrytography.HashAlgoStd(publickey);
            trans.Sender = address;
            trans.TransHash = BLCrytography.HashTransaction(trans);
         
         

          if(ValidateTrans(trans, publickey)==true)
            {
                //add it to pool after validation
                app.MemPool.Add(trans);
                BroadCastTrans(trans);

                //You should pickit first from the queue
                ConfirmTrans(trans, publickey);
            }

        }
        public bool ValidateTrans(LedgerTransModel trans, string publickey)
        {
            trans.Fee = 1;
            double output = 0;
            
            var allinputs = xexplorer.GetUnpent(publickey);
            foreach (var vin in allinputs)
            {
                vin.Spent = 1;

                trans.Inputs.Add(vin);

                output += vin.Amount;

                if (output >= trans.Amount)
                {
                    break;
                }
                
            }



            if (trans.Inputs.Sum(x => x.Amount) < trans.Amount)
            {
                //not enough balance
                return false;
            }

            //Validate Key
            //Retrieve un spent UTXO
            //Generate new utxo
            //Calc fee
          
            return true;
        }
        public void ConfirmTrans(LedgerTransModel trans, string publickey)
        {
          xutxo.  GenerateOutput(trans);
            xutxo.GenerateReceiverOutput(trans);
            xutxo.GenerateFeeOutput(trans);

            xledger.SaveLedgerTrans(trans);
            var lastblock = GetLastBlock();
            lastblock.Transactions.Add(trans);

            app.MemPool.Remove(trans);
        }

   
        public void BroadCastTrans(LedgerTransModel trans)
        {

        }
    }
}
