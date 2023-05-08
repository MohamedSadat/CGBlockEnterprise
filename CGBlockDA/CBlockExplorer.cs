using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CGBlockDA
{
    public class CBlockExplorer
    {
        public static List<LedgerTransModel>GetTransByAddress(string address)
        {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select o.*    ");
                    sql.Append("from LedgerTrans o ");
                sql.Append("where o.Sender=@Sender");
                var p = new { Sender = address };
                    var r = connection.Query<LedgerTransModel>(sql.ToString(),p).ToList();
                    return r;
                }
            
        }
        public static async Task< List<LedgerTransModel>> GetTransByAddressAsync(string address)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select o.*    ");
                sql.Append("from LedgerTrans o ");
                sql.Append("where o.Sender=@Sender");
                var p = new { Sender = address };
                var r = (await connection.QueryAsync<LedgerTransModel>(sql.ToString(), p)).ToList();
                return r;
            }

        }
    }
}
