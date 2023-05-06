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
    public class CUTXOExplorer
    {
        public List<UTXOModel> GetUnpent(string publickey)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select UTXO.*    ");
                sql.Append("from UTXO o ");
                sql.Append(" where o.publickey=@publickey ");
                sql.Append(" and o.Spent=@Spent ");

                var p = new { publickey = publickey, Spent=0 };

                var r = connection.Query<UTXOModel>(sql.ToString(), p).ToList();
                return r;
            }
        }
  
    }
}
