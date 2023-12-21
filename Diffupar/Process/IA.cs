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
            Log.Write.WriteError("================================================================");
            Log.Write.WriteError("================================================================");
            Log.Write.WriteError("Inicio Schedule");
            string _query = "select id, name from dbSchedule where status = 1 order by orden";
            try
            {
                foreach (DataRow r in ADO.SQL.SqlExecuteQueryDataSet(_query, _sqlConnection).Tables[0].Rows)
                {
                    int _scheduleID = int.Parse(r[0].ToString());
                    string _scheduleName = r[1].ToString();

                    Log.Write.WriteError(string.Format("Ejecutando Schedule: {0}", _scheduleName.ToUpper()));


                    ProcessEntity _pe = MappingEntities.Mapping.GetProcessEntityBySchedule(_scheduleID);

                    Log.Write.WriteError(_pe.queryExceute);
                    Log.Write.WriteError(_pe.QueryId.ToString());
                    Log.Write.WriteError(_pe.ProviderName);
                    Log.Write.WriteError(_pe.TableOrigenName);
                    Log.Write.WriteError(_pe.TableDestinoName);
                    Log.Write.WriteError(_pe.Where);
                    Log.Write.WriteError(_pe.DbSourceConnection);
                    Log.Write.WriteError(_pe.DbDestinoConnection);

                    Log.Write.WriteError("Importando datos");
                    Log.Write.WriteError(string.Format("Total Registrosd: {0}", _pe.TdataImport.Rows.Count));
                    ADO.SQL.SqlBulkCopy(_pe.TableDestinoName, _pe.TdataImport, _pe.ColumnMapping, _sqlConnection);
                    Log.Write.WriteError("OK");
                }

                Log.Write.WriteError("Actualiando Warehouse");
                ADO.SQL.SqlExecuteNonQuery("exec [stg].[Sp_ImportDimensiones]", _sqlConnection);
                Log.Write.WriteError("OK");
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
                Console.ReadLine();
            }
            finally {
                Log.Write.WriteError("Fin");
            }

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
