using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace ADO
{
    public class SQL
    {
        public static DataSet SqlExecuteQueryDataSet(string query, string strConexion)
        {
            DataSet _ds = new DataSet();
            using (SqlCommand _sqlCommand = new SqlCommand(query, (new SqlConnection(strConexion))))
            {
                _sqlCommand.CommandTimeout = 6000;
                (new SqlDataAdapter(_sqlCommand)).Fill(_ds);
            }

            return _ds;
        }

        public static DataSet SqlExecuteProcedureDataSet(string ProceduereName,IList<SqlParameter> param, string strConexion)
        {
            DataSet _ds = new DataSet();
            using (SqlCommand _sqlCommand = new SqlCommand())
            {                
                _sqlCommand.Connection = new SqlConnection(strConexion);
                _sqlCommand.CommandTimeout = 6000;

                _sqlCommand.CommandText = ProceduereName;
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                foreach(SqlParameter _sqlp in param) 
                {
                    _sqlCommand.Parameters.Add(_sqlp);
                }
                
                (new SqlDataAdapter(_sqlCommand)).Fill(_ds);
            }

            return _ds;
        }

        public static void SqlBulkCopy(string destinationTableName, DataTable tData, IList<SqlBulkCopyColumnMapping> mappings, string strConexion)
        {
            SqlConnection _sconn = new SqlConnection(strConexion);

            try
            {
                //Vacia la tabla destino
                string _query = string.Format("delete {0}", destinationTableName);
                SqlExecuteNonQuery(_query, strConexion);


                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_sconn))
                {
                    bulkCopy.DestinationTableName = destinationTableName;
                    bulkCopy.BulkCopyTimeout = 2000;
                    //bulkCopy.BatchSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BatchSize"]);
                    foreach (SqlBulkCopyColumnMapping _mapping in mappings)
                    {
                        bulkCopy.ColumnMappings.Add(_mapping);
                    }
                    _sconn.Open();
                    bulkCopy.WriteToServer(tData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sconn.Close();
                _sconn.Dispose();
               // Log.Write.WriteError(String.Format("Registros Importados: {0}", tData.Rows.Count));
            }
        }


        public static void SqlExecuteNonQuery(string query, string strConexion)
        {
            using (SqlCommand _sqlCommand = new SqlCommand(query, (new SqlConnection(strConexion))))
            {
                _sqlCommand.CommandTimeout = 6000;
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

    }
}
