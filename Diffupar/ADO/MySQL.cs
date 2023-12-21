using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace ADO
{
    public class MySQL
    {
        public static DataSet MySqlExecuteQueryDataSet(string query, string strConexion)
        {
            DataSet _ds = new DataSet();
            using (MySqlCommand _sqlCommand = new MySqlCommand(query, (new MySqlConnection(strConexion))))
            {
                _sqlCommand.CommandTimeout = 6000;
                (new MySqlDataAdapter(_sqlCommand)).Fill(_ds);
            }

            return _ds;
        }

        public static void MySqlExecuteNonQuery(string query, string strConexion)
        {
            using (MySqlCommand _sqlCommand = new MySqlCommand(query, (new MySqlConnection(strConexion))))
            {
                _sqlCommand.CommandTimeout = 6000;
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

    }
}
