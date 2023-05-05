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

            transtable.TransId = GetTransId() + 1;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                var p = new DynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into LedgerTrans ");
                sql.Append("(TransId,Sender,Receiver,Amount ,Fee,Reward  ,BlockHash ,TransDate ,Note,TransHash)");
                sql.Append("values");
                sql.Append("(@TransId,@Sender,@Receiver,@Amount ,@Fee,@Reward  ,@BlockHash ,@TransDate ,@Note,@TransHash)");
                sql.Append("");
                sql.Append("");
                var sqlLedgerupdate = "Update LedgerTrans set TransHash=@TransHash where RecId=@RecId and Company=@Company";



                using (var trans = connection.BeginTransaction())
                {
                    try
                    {


                        var r = connection.Execute(sql.ToString(), transtable, trans);
                        // connection.Execute(sqlLedgerupdate, transtable, trans);
                      
                            foreach (var vin in transtable.Inputs)
                            {

                            }
                            foreach (var vout in transtable.Outputs)
                            {

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
