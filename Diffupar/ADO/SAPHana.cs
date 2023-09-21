using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public class SAPHana
    {
        //static string _strConnetion = "\"Dsn=Sap; uid = alft; pwd = aLf4951*;\"";

        /// <summary>
        /// Retorna los datos de query suministrada.
        /// Para usar nombre de columns, las mismas tienen que estar entre comillas dobles        
        /// </summary>
        /// <remarks>
        ///  Ej:
        /// "SELECT \"CardCode\"  FROM DIFFUPARSA.OCRD where \"CardCode\" = 'CL-000976';"
        /// </remarks>
        /// <param name="query"></param>        
        /// <returns>System.Data.DataSet</returns>
        public static DataSet getByQuery(string query, string strSapConnection)
        {
            DataSet ds = new DataSet();
            OdbcConnection OdbcConn = new OdbcConnection(strSapConnection);

            try
            {
                using (OdbcCommand command = OdbcConn.CreateCommand())
                {   
                    command.CommandText = query;
                    OdbcConn.Open();

                    OdbcDataAdapter adapter = new OdbcDataAdapter(command);

                    adapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                OdbcConn.Close();
                OdbcConn.Dispose();
            }

            return ds;
        }
    }
}
