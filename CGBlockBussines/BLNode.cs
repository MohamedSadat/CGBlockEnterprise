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
        INode xnode = new CNodeApp();
        private readonly IApp app;
        BLBlock xblock;
    //    public NodeModel Node { get; set; } = new NodeModel();
        BLUTXO xutxo;
        public BLNode(IApp app)
        {
            this.app = app;
          
            xblock=new BLBlock(app);
            xutxo=new BLUTXO(app);
        }

 
        public void BroadCast(BlockModel block)
        {
            foreach(var peer in app.Node.Peers)
            {

            }
        }

        public void GenerateGenisusBlock()
        {
            var block = new BlockModel
            {
                PrevBlockHash = "0000000000000000000000000000000000000000000000000000000000000000",
                BlockHash = "0000000000000000000000000000000000000000000000000000000000000000",
                TimeStamp = DateTime.Now.Ticks,
                BlockHeight = 0,
                Fee = 0,
                Reward = 50,
                NodeAddress = app.Node.Address
            };
            var trans = new LedgerTransModel
            {
                BlockHash = block.BlockHash,
                TimeStamp = DateTime.Now.Ticks,
                TransDate = DateTime.Now,
                Receiver = app.Node.Address,
                Amount = 50,
                NodeAddress = app.Node.Address,
                Note = "Kassad Genisus block",
                Fee = 0,
                Reward=0,
                Sender = "",
                BlockHeight=block.BlockHeight,
                PublicKey=app.Node.PublicKey,
            
            };
            BLCrytography.HashTransaction(trans);
           
            block.Transactions.Add(trans);

            app.Node.BlockChain.Add(new BlockChainModel
            {
                BlockHash = block.BlockHash,
                PrevBlockHash = block.PrevBlockHash,
                BlockHeight = block.BlockHeight,
                BlockHashModel=block
            });

            var utxo = new UTXOModel
            {
                Address = app.Node.Address,
                PublicKey = app.Node.PublicKey,
                Id = 0,
                TransId = trans.TransId,
                OutputIndex = 0,
                Amount=trans.Amount,
                Spent=0
               
            };
            trans.Outputs.Add(utxo);
            xnode.SaveGenisusBlock(block);
        }
        public void GenerateBlock()
        {
            var block = new BlockModel
            {
                PrevBlockHash = app.Node.LastBlock.BlockHash,
                TimeStamp = DateTime.Now.Ticks,
                BlockHeight = app.Node.LastBlock.BlockHeight+1,
                NodeAddress=app.Node.Address,
                BlockDate=DateTime.Now,
                Reward=50,
            };

            BLCrytography.HashBlock(block);

            //Generate Reward transaction
          block.Transactions.Add(  GenerateRewardTransaction(block));

            app.Node.BlockChain.Add(new BlockChainModel
            {
                BlockHash = block.BlockHash,
                PrevBlockHash = block.PrevBlockHash,
                BlockHeight = block.BlockHeight,
                BlockHashModel = block
            });
            if(xnode.SaveBlock(block)==true)
            {
                app.Node.CurrentBlock = block;
            }

        }
        public LedgerTransModel GenerateRewardTransaction(BlockModel block)
        {
            var trans = new LedgerTransModel
            {
                Amount = 50,
                BlockHash = block.BlockHash,
                Fee = 0,
                Note = "Reward",
                Receiver = block.NodeAddress,
                TransDate = DateTime.Now,
                TimeStamp = DateTime.Now.Ticks,
                BlockHeight=block.BlockHeight,
                PublicKey=app.Node.PublicKey
            };
            BLCrytography.HashTransaction(trans);
            var output = new UTXOModel
            {
                Address = block.NodeAddress,
                Amount = trans.Amount,
                OutputIndex = 0,
                PublicKey = app.Node.PublicKey,
                Spent = 0,
                TransId = trans.TransId
            };
            trans.Outputs.Add(output);
            return trans;

        }
        public void ReceiveTransaction(LedgerTransModel trans)
        {
            if(app.Node.CurrentBlock.BlockState==CGBlockInfra.CGTypes.TBlockState.NotSet)
            {
                GenerateBlock();
            }
          //  var address = BLCrytography.HashAlgoStd(publickey);
            trans.BlockHash = app.Node.CurrentBlock.BlockHash;
            trans.BlockHeight = app.Node.CurrentBlock.BlockHeight;
           // trans.Sender = address;
        BLCrytography.HashTransaction(trans);
         
         

          if(ValidateTrans(trans)==true)
            {
                trans.TransStatus = CGBlockInfra.CGTypes.TTransState.Validated;
                //add it to pool after validation
                app.Node.MemPool.Add(trans);
                trans.TransStatus = CGBlockInfra.CGTypes.TTransState.Pending;
               // BroadCastTrans(trans);

                //You should pickit first from the queue
                ConfirmTrans(trans);
            }

        }
        public bool ValidateTrans(LedgerTransModel trans)
        {
            trans.Fee = 1;
            double output = 0;
            
            if(xnode.GetBalance(trans.PublicKey)<trans.Amount)
            {
                return false;
                //not enough balance

            }
        
          
            return true;
        }
        public void ConfirmTrans(LedgerTransModel trans)
        {
            xutxo.GenerateInputs(trans);
          xutxo.  GenerateOutput(trans);
            xutxo.GenerateReceiverOutput(trans);
            xutxo.GenerateFeeOutput(trans);

            xnode.SaveLedgerTrans(trans);

            app.Node.CurrentBlock.Transactions.Add(trans);

            app.Node.MemPool.Remove(trans);
        }

   
        public void BroadCastTrans(LedgerTransModel trans)
        {

        }
    }
}
