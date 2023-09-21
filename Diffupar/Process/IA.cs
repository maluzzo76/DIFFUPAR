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
            string _query = "select id, name from dbSchedule where status = 1";

            foreach (DataRow r in ADO.SQL.SqlExecuteQueryDataSet(_query, _sqlConnection).Tables[0].Rows)
            {
                int _scheduleID = int.Parse(r[0].ToString());
                string _scheduleName = r[1].ToString();

                Console.WriteLine(string.Format("Ejecutando Schedule: {0}",_scheduleName.ToUpper()));

                ProcessEntity _pe = MappingEntities.Mapping.GetProcessEntityBySchedule(_scheduleID);

                Console.WriteLine(_pe.queryExceute);
                Console.WriteLine(_pe.QueryId);
                Console.WriteLine(_pe.ProviderName);
                Console.WriteLine(_pe.TableOrigenName);
                Console.WriteLine(_pe.TableDestinoName);
                Console.WriteLine(_pe.Where);
                Console.WriteLine(_pe.DbSourceConnection);
                Console.WriteLine(_pe.DbDestinoConnection);
                Console.WriteLine("Listando datos");
                foreach (DataRow row in _pe.TdataImport.Rows)
                {
                    string _srow = "";
                    foreach (DataColumn c in _pe.TdataImport.Columns)
                    {
                        _srow += string.Format("{0}\t", row[c.ColumnName].ToString());
                    }
                    Console.WriteLine(_srow);
                }

                Console.WriteLine("Importando datos");
                ADO.SQL.SqlBulkCopy(_pe.TableDestinoName, _pe.TdataImport, _pe.ColumnMapping, _sqlConnection);
            }

            Console.WriteLine("Fin");

            Console.ReadLine();
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
