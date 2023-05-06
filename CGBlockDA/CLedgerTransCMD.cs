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


                        var r = connection.Execute(sqltrans.ToString(), transtable, trans);
                        // connection.Execute(sqlLedgerupdate, transtable, trans);
                        var pinput = new { Spent = 1, publickey = "" };
                        foreach (var vin in transtable.Inputs)
                        {
                            connection.Execute(sqlinput, pinput, trans);
                        }
                        foreach (var vout in transtable.Outputs)
                        {
                            connection.Execute(sqlinput, vout, trans);
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
        public static int GetTransId()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"select Max(TransId) from ");
                sql.Append("LedgerTrans  ");
             
                var r = connection.ExecuteScalar<int>(sql.ToString());
                return r;
            }

        }
    }
}
