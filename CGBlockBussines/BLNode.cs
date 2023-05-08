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
                NodeAddress = app.Node.Address,BlockDate=DateTime.Now,
                BlockSize = 0,
                Volume = 0,
                Version = 0,
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
                TransType = CGBlockInfra.CGTypes.TTransType.CoinBase

            };
            trans.TransHash= BLCrytography.HashTransaction(trans);
           
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
            BLCrytography.CalculateMerkleRoot(block);
            xnode.SaveGenisusBlock(block);
        }
        public void GenerateNewBlock()
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

           block.BlockHash= BLCrytography.MineBlock(block);

            //Generate Reward transaction
          block.Transactions.Add(  GenerateRewardTransaction(block));
            BLCrytography.CalculateMerkleRoot(block);

            app.Node.BlockChain.Add(new BlockChainModel
            {
                BlockHash = block.BlockHash,
                PrevBlockHash = block.PrevBlockHash,
                BlockHeight = block.BlockHeight,
                BlockHashModel = block,
                
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
                PublicKey=app.Node.PublicKey,
                TransType=CGBlockInfra.CGTypes.TTransType.CoinBase,
                
            };
          trans.TransHash=  BLCrytography.HashTransaction(trans);
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
            if(app.Node.CurrentBlock.BlockState==CGBlockInfra.CGTypes.TBlockState.NotSet || app.Node.CurrentBlock.Transactions.Count>=50)
            {
                GenerateNewBlock();
            }
          //  var address = BLCrytography.HashAlgoStd(publickey);
            trans.BlockHash = app.Node.CurrentBlock.BlockHash;
            trans.BlockHeight = app.Node.CurrentBlock.BlockHeight;
           // trans.Sender = address;
       trans.TransHash= BLCrytography.HashTransaction(trans);
         
         

          if(ValidateTrans(trans)==true)
            {
                trans.TransStatus = CGBlockInfra.CGTypes.TTransState.Validated;
                //add it to pool after validation
                app.Node.MemPool.Push(trans);
                trans.TransStatus = CGBlockInfra.CGTypes.TTransState.Pending;
               // BroadCastTrans(trans);

                //You should pickit first from the queue
                ConfirmTrans();
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
        public void ConfirmTrans()
        {
            if (app.Node.MemPool.Count == 0)
                return;

            LedgerTransModel trans = app.Node.MemPool.Pop();
            xutxo.GenerateSenderInputs(trans);
          xutxo.  GenerateSenderOutput(trans);

            xutxo.GenerateReceiverOutput(trans);
            xutxo.GenerateFeeOutput(trans);

            xnode.SaveLedgerTrans(trans);

            app.Node.CurrentBlock.Transactions.Add(trans);
            CheckBlock(app.Node.CurrentBlock);
            BLCrytography.CalculateMerkleRoot(app.Node.CurrentBlock);
            xnode.UpdateMerkleRoot(app.Node.CurrentBlock);

        }

   public void CheckBlock(BlockModel block)
        {
            if(block.Transactions.Count==10)
            {
                //Validated
                //GenerateNew
                GenerateNewBlock();
            }
        }
        public void BroadCastTrans(LedgerTransModel trans)
        {

        }
    }
}
