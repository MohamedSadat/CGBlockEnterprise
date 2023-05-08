using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGBlockInfra.Models;

namespace CGBlockDA
{
    public class CLedgerTransCMD
    {
        public bool SaveLedgerTrans(LedgerTransModel transtable)
        {

           // transtable.TransId = GetTransId() + 1;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                var p = new DynamicParameters();
                StringBuilder sqltrans = new StringBuilder();
                sqltrans.Append("insert into LedgerTrans ");
                sqltrans.Append("(TransId,Sender,Receiver,Amount ,Fee,Reward  ,BlockHash ,TransDate ,Note,TransHash,BlockHeight)");
                sqltrans.Append("values");
                sqltrans.Append("(@TransId,@Sender,@Receiver,@Amount ,@Fee,@Reward  ,@BlockHash ,@TransDate ,@Note,@TransHash,@BlockHeight)");
                sqltrans.Append("");
                sqltrans.Append("");
                var sqlinput = "Update UTXO set Spent=@Spent,OutputIndex=@OutputIndex where  Id=@Id ";
                var sqloutputs = "insert into UTXO(Id,TransId,OutputIndex,Amount,Address,PublicKey,Spent) values(@Id,@TransId,@OutputIndex,@Amount,@Address,@PublicKey,@Spent)";
                connection.Open();
                int id = GetUTXOId();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {

                    

                        var r = connection.Execute(sqltrans.ToString(), transtable, trans);
                        transtable.TransStatus = CGBlockInfra.CGTypes.TTransState.Created;
                        // connection.Execute(sqlLedgerupdate, transtable, trans);
                     
                        foreach (var vout in transtable.Outputs)
                        {
                            vout.Id = id;
                            id++;
                            connection.Execute(sqloutputs, vout, trans);
                        }

                        foreach (var vin in transtable.Inputs)
                        {
                        
                            var pinput = new { Spent = 1, publickey =transtable.PublicKey,Id=vin.Id,OutputIndex=0 };

                            // vin.Id = id;
                            //  id++;
                            connection.Execute(sqlinput, pinput, trans);
                        }

                   

                        

                        trans.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        transtable.ErrorMSG = ex.Message;
                        return false;
                    }

                }
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
                return r + 1;
            }

        }

 
    }
}
