using Process.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.MappingEntities
{
    internal class Mapping
    {
        public static string _sqlConnection = ConfigurationManager.ConnectionStrings["dbHost"].ConnectionString;

        public static ProcessEntity GetProcessEntityBySchedule(int scheduleId)
        { 
            ProcessEntity _pe = new ProcessEntity();

            DataSet _dsSchedule = GetSchedule(scheduleId);

            // si no se encontro el schedule se retorna la entidad vacia
            if (_dsSchedule.Tables[0].Rows.Count <1)
                return _pe;

            //Mapear Entidad
            _pe.QueryId = int.Parse(_dsSchedule.Tables[0].Rows[0]["queryID"].ToString());
            _pe.ProviderName = _dsSchedule.Tables[0].Rows[0]["providerName"].ToString().ToUpper();
            _pe.TableDestinoName = _dsSchedule.Tables[0].Rows[0]["tableDestino"].ToString();
            _pe.TableOrigenName = _dsSchedule.Tables[0].Rows[0]["tableOrgien"].ToString();
            _pe.Where = _dsSchedule.Tables[0].Rows[0]["Where"].ToString();
            _pe.DbSourceConnection = _dsSchedule.Tables[0].Rows[0]["sourceConnection"].ToString();
            _pe.DbDestinoConnection = _dsSchedule.Tables[0].Rows[0]["destinoConnection"].ToString();

            //Mapea las columnas
            string _columns = string.Empty;
            _pe.ColumnMapping = new List<SqlBulkCopyColumnMapping>();
            foreach (DataRow _row in _dsSchedule.Tables[1].Rows)
            {
                
                _pe.ColumnMapping.Add(new SqlBulkCopyColumnMapping(_row["columnOrigen"].ToString(), _row["ColumnDestino"].ToString()));
                _columns  += string.Format("{0},", GetColumnNameByExecute(_row["columnOrigen"].ToString(),_pe.ProviderName));
            }


            _pe.queryExceute = string.Format("select  {0} from {1} {2};", _columns.Substring(0, _columns.Length - 1), _pe.TableOrigenName, _pe.Where);
            _pe.TdataImport = GetDataImport(_pe);
            
             return _pe;
        }

        private static DataSet GetSchedule(int scheduleId) 
        {
            DataSet _ds = new DataSet();

            IList<SqlParameter> _param = new List<SqlParameter>();
            _param.Add(new SqlParameter("scheduleid", scheduleId));

            _ds = ADO.SQL.SqlExecuteProcedureDataSet("GetSchedule", _param, _sqlConnection);
                        
            return _ds;
        }

        private static DataTable GetDataImport(ProcessEntity processEnt)
        {
            DataTable _dtResult = new DataTable();

            switch (processEnt.ProviderName)
            {
                case "SAP":
                    DataSet _ds = ADO.SAPHana.getByQuery(processEnt.queryExceute, processEnt.DbSourceConnection);
                    if(_ds.Tables.Count>0)
                        _dtResult = _ds.Tables[0];
                    break;

                default:
                    return _dtResult;
            }

            return _dtResult;
        }

        private static  string GetColumnNameByExecute(string columnName, string ProviderName)
        {
            string _result = columnName;

            switch (ProviderName)
            {
                case "SAP":
                    _result = string.Format("\"{0}\"", columnName);
                    break;
            }

            return _result;
        }

    }
}
