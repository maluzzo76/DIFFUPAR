using Process.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process
{
    public class IA
    {
        public static string _sqlConnection = ConfigurationManager.ConnectionStrings["dbHost"].ConnectionString;
        private static IList<SqlBulkCopyColumnMapping> _mappingsColumns = new List<SqlBulkCopyColumnMapping>();

        public static void ScheduleExcecute()
        {


        }

        public static void ProcesarComplementos()
        {
            ComplementosProcess.Procesar(_sqlConnection);
        }


        /*
        public static string GetQueryByTableID(int tableid)
        {
            IList<SqlParameter> _param = new List<SqlParameter>();
            _param.Add(new SqlParameter("@Tid", tableid));

            DataSet _ds = ADO.SQL.SqlExecuteProcedureDataSet("dbo.DbGetQuery", _param, _sqlConnection);

            string _provider = string.Empty;
            string _Table = string.Empty;
            string _columns = string.Empty;

            foreach (DataRow _row in _ds.Tables[0].Rows)
            {
                _provider = _row["providerName"].ToString();
                _Table = _row["tableName"].ToString();
                _columns += string.Format("{0},", _row["ColumnName"].ToString());
            }

            return string.Format("select {0} from {1};|{2}", _columns.Substring(0, _columns.Length - 1), _Table,_provider);
        }

        public static void BulkInsert(string destinationTableName, DataTable dtabe)
        {

            _mappingsColumns.Clear();
            
            _mappingsColumns.Add(new SqlBulkCopyColumnMapping("ItemCode", "ItemCode"));
            _mappingsColumns.Add(new SqlBulkCopyColumnMapping("ItemName", "Itemname"));

            ADO.SQL.SqlBulkCopy(destinationTableName, dtabe, _mappingsColumns, _sqlConnection);
        }
        */
    }
}
