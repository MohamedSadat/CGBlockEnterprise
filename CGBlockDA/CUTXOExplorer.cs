using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Security.Cryptography.X509Certificates;

namespace CGBlockDA
{
    public class CUTXOExplorer
    {
        public List<UTXOModel> GetUnpent(string Address)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select o.*    ");
                sql.Append("from UTXO o ");
                sql.Append(" where o.Address=@Address ");
                sql.Append(" and o.Spent=@Spent ");

                var p = new { Address = Address, Spent=0 };

                var r = connection.Query<UTXOModel>(sql.ToString(), p).ToList();
                return r;
            }
        }

        public static double GetBalance(string Address)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.cn))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"select sum(Amount)  ");
                sql.Append("from UTXO o ");
                sql.Append(" where o.Address=@Address ");
                sql.Append(" and o.Spent=@Spent ");
                var p = new { Address = Address, Spent = 0 };

                var r = connection.ExecuteScalar<double>(sql.ToString(),p);
                return r;
            }

        }

    }
}
