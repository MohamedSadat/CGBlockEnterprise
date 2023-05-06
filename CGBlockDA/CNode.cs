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
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into Blocks ");
                sql.Append("(PrevBlockHash,TimeStamp,BlockHeight,BlockHash ,NodeAddress)");
                sql.Append("values");
                sql.Append("(@PrevBlockHash,@TimeStamp,@BlockHeight,@BlockHash ,@NodeAddress)");
                sql.Append("");
                sql.Append("");
                var sqlLedgerupdate = "Update LedgerTrans set TransHash=@TransHash where RecId=@RecId and Company=@Company";



                using (var trans = connection.BeginTransaction())
                {
                    try
                    {


                        var r = connection.Execute(sql.ToString(), block, trans);
                        connection.Execute(sqlLedgerupdate, block, trans);
                        foreach(var ledgertrans in block.Transactions)
                        {
                            foreach(var vin in ledgertrans.Inputs)
                            {

                            }
                            foreach (var vout in ledgertrans.Outputs)
                            {

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
                sqlblock.Append("(PrevBlockHash,TimeStamp,BlockHeight,BlockHash ,NodeAddress)");
                sqlblock.Append("values");
                sqlblock.Append("(@PrevBlockHash,@TimeStamp,@BlockHeight,@BlockHash ,@NodeAddress)");
                sqlblock.Append("");
                sqlblock.Append("");
                StringBuilder sqltrans = new StringBuilder();
                sqltrans.Append("insert into LedgerTrans ");
                sqltrans.Append("(TransId,Sender,Receiver,Amount ,Fee,Reward  ,BlockHash ,TransDate ,Note,TransHash)");
                sqltrans.Append("values");
                sqltrans.Append("(@TransId,@Sender,@Receiver,@Amount ,@Fee,@Reward  ,@BlockHash ,@TransDate ,@Note,@TransHash)");
                sqltrans.Append("");
                sqltrans.Append("");
                var sqlinput = "Update UTXO set Spent=@Spent where publickey=@publickey ";
                var sqloutputs = "insert into UTXO() values()";


                using (var trans = connection.BeginTransaction())
                {
                    try
                    {


                        var r = connection.Execute(sqlblock.ToString(), block, trans);
                        foreach (var ledgertrans in block.Transactions)
                        {
                            connection.Execute(sqltrans.ToString(), ledgertrans,trans);
                            foreach (var vin in ledgertrans.Inputs)
                            {

                            }
                            foreach (var vout in ledgertrans.Outputs)
                            {
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
    }
}
