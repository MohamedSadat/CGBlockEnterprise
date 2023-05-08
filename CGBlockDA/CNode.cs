using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGBlockInfra.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace CGBlockDA
{
    public class CNode
    {

        public bool SaveGenisusBlock(BlockModel block)
        {

       
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                var p = new DynamicParameters();
                StringBuilder sqlblock = new StringBuilder();
                sqlblock.Append("insert into Blocks ");
                sqlblock.Append("(PrevBlockHash,TimeStamp,BlockHeight,BlockHash ,NodeAddress,Reward,BlockDate,MerkleRoot)");
                sqlblock.Append("values");
                sqlblock.Append("(@PrevBlockHash,@TimeStamp,@BlockHeight,@BlockHash ,@NodeAddress,@Reward,@BlockDate,@MerkleRoot)");
                sqlblock.Append("");
                sqlblock.Append("");
                StringBuilder sqltrans = new StringBuilder();
                sqltrans.Append("insert into LedgerTrans ");
                sqltrans.Append("(TransId,Sender,Receiver,Amount ,Fee,Reward  ,BlockHash ,TransDate ,Note,TransHash,BlockHeight)");
                sqltrans.Append("values");
                sqltrans.Append("(@TransId,@Sender,@Receiver,@Amount ,@Fee,@Reward  ,@BlockHash ,@TransDate ,@Note,@TransHash,@BlockHeight)");

                var sqloutputs = "insert into UTXO(Id,TransId,OutputIndex,Amount,Address,PublicKey,Spent) values(@Id,@TransId,@OutputIndex,@Amount,@Address,@PublicKey,@Spent)";


                var sqlblockchain = "insert into BlockChain(BlockHash,PrevBlockHash,BlockHeight)values(@BlockHash,@PrevBlockHash,@BlockHeight)";

                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {

                        var pchain =new { BlockHash=block.BlockHash, PrevBlockHash =block.PrevBlockHash, BlockHeight =block.BlockHeight};
                        var r = connection.Execute(sqlblock.ToString(), block, trans);
                        block.BlockState = CGBlockInfra.CGTypes.TBlockState.Saved;
                        connection.Execute(sqlblockchain, pchain, trans);
                        foreach(var ledgertrans in block.Transactions)
                        {
                                connection.Execute(sqltrans.ToString(), ledgertrans, trans);
                            ledgertrans.TransStatus = CGBlockInfra.CGTypes.TTransState.Completed;

                            foreach (var vout in ledgertrans.Outputs)
                            {
                                connection.Execute(sqloutputs, vout, trans);

                            }

                        }
                        trans.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {

                        return false;
                    }

                }
            }


        }
        public bool SaveBlock(BlockModel block)
        {


            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                var p = new DynamicParameters();
                StringBuilder sqlblock = new StringBuilder();
                sqlblock.Append("insert into Blocks ");
                sqlblock.Append("(PrevBlockHash,TimeStamp,BlockHeight,BlockHash ,NodeAddress,Reward,BlockDate,MerkleRoot,Nonce)");
                sqlblock.Append("values");
                sqlblock.Append("(@PrevBlockHash,@TimeStamp,@BlockHeight,@BlockHash ,@NodeAddress,@Reward,@BlockDate,@MerkleRoot,@Nonce)");
                sqlblock.Append("");
                sqlblock.Append("");
                StringBuilder sqltrans = new StringBuilder();
                sqltrans.Append("insert into LedgerTrans ");
                sqltrans.Append("(TransId,Sender,Receiver,Amount ,Fee,Reward  ,BlockHash ,TransDate ,Note,TransHash,BlockHeight)");
                sqltrans.Append("values");
                sqltrans.Append("(@TransId,@Sender,@Receiver,@Amount ,@Fee,@Reward  ,@BlockHash ,@TransDate ,@Note,@TransHash,@BlockHeight)");
                sqltrans.Append("");
                sqltrans.Append("");
                var sqlinput = "Update UTXO set Spent=@Spent where publickey=@publickey ";
                var sqloutputs = "insert into UTXO(Id,TransId,OutputIndex,Amount,Address,PublicKey,Spent) values(@Id,@TransId,@OutputIndex,@Amount,@Address,@PublicKey,@Spent)";

                var sqlblockchain = "insert into BlockChain(BlockHash,PrevBlockHash,BlockHeight)values(@BlockHash,@PrevBlockHash,@BlockHeight)";

                connection.Open();  
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {


                        var r = connection.Execute(sqlblock.ToString(), block, trans);
                        block.BlockState = CGBlockInfra.CGTypes.TBlockState.Saved;

                        var pchain = new { BlockHash = block.BlockHash, PrevBlockHash = block.PrevBlockHash, BlockHeight = block.BlockHeight };
                      connection.Execute(sqlblockchain, pchain, trans);

                        foreach (var ledgertrans in block.Transactions)
                        {
                            connection.Execute(sqltrans.ToString(), ledgertrans,trans);
                            ledgertrans.TransStatus = CGBlockInfra.CGTypes.TTransState.Completed;
                            foreach (var vin in ledgertrans.Inputs)
                            {
                                vin.Id = GetUTXOId();
                                connection.Execute(sqloutputs.ToString(), vin, trans);

                            }
                            foreach (var vout in ledgertrans.Outputs)
                            {
                                vout.Id = GetUTXOId();
                                connection.Execute(sqloutputs.ToString(), vout, trans);
                            }

                        }
                        trans.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {

                        return false;
                    }

                }
            }


        }
        public static int GetBlockHeight()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"select Max(BlockHeight) from ");
                sql.Append("Blocks  ");

                var r = connection.ExecuteScalar<int>(sql.ToString());
                return r;
            }

        }
        public List<NodeModel> GetPeers()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select o.*    ");
                sql.Append("from Peers o ");
            
                var r = connection.Query<NodeModel>(sql.ToString()).ToList();
                return r;
            }
        }
        public NodeModel GetNode()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select o.*    ");
                sql.Append("from Node o ");

                var r = connection.Query<NodeModel>(sql.ToString()).FirstOrDefault();
                return r;
            }
        }

        public List<BlockChainModel> GetBlockChain()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select o.*    ");
                sql.Append("from BlockChain o ");

                var r = connection.Query<BlockChainModel>(sql.ToString()).ToList();
                return r;
            }
        }
        public BlockModel GetBlock(int blockheight)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select o.*    ");
                sql.Append("from Blocks o ");
                sql.Append("where blockheight=@blockheight");
                var sqltrans = "select t.* from LedgerTrans t where blockheight=@blockheight ";
                var p = new { blockheight= blockheight };
                var r = connection.Query<BlockModel>(sql.ToString(),p).FirstOrDefault();
                r.BlockState = CGBlockInfra.CGTypes.TBlockState.Saved;
               r.Transactions = connection.Query<LedgerTransModel>(sqltrans, p).ToList();

                return r;
            }
        }
        public static int GetUTXOId()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"select Max(Id) from ");
                sql.Append("UTXO  ");

                var r = connection.ExecuteScalar<int>(sql.ToString());
                return r+1;
            }

        }

        public void UpdateMerkleRoot(BlockModel block)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update Blocks set MerkleRoot=@MerkleRoot ");
                sql.Append("where BlockHeight=@BlockHeight ");

                var r = connection.Execute(sql.ToString(),block);
                
            }
        }
    }
}
