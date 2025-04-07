using Process.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Process
{
    public class ComplementosProcess
    {
        static string _sqlConnection = "";
        static string _mySqlCegid = ConfigurationManager.ConnectionStrings["CegidMysql"].ConnectionString;
        public static void Procesar(string sqlConnection)
        {
            try
            {
                _sqlConnection = sqlConnection;
                string _query = "exec [dbo].[GetDbProcess]";
                DataSet _dsProcess = ADO.SQL.SqlExecuteQueryDataSet(_query, sqlConnection);

                foreach (DataRow _r in _dsProcess.Tables[0].Rows)
                {
                    int _id = Convert.ToInt32(_r["ID"]);

                    switch (_r["TipoProceso"].ToString())
                    {
                        case "Libro Mayor":
                            LibroMayor(_r);
                            break;

                        case "Objetivos Totales":
                            ObjetivosTotales(_r);
                            break;

                        case "Objetivos Diffupar":
                            ObjetivosDiffupar(_r);
                            break;

                        //Duplicar este metodo y cambiar el valore del case y el nombre de la tabla de destino
                        case "Plan de Cuentas":
                            ComplementoGenerico(_r, "stg.PlandecuentasComplemento");
                            break;

                        case "Vendedores":
                            ComplementoGenerico(_r, "stg.VendedoresComplemento");
                            string _queryComplementoUpdate = "exec [whs].[Sp_ActualizarComplementos]";
                            ADO.SQL.SqlExecuteNonQuery(_queryComplementoUpdate, sqlConnection);
                            break;

                        case "ProductosRelevantes":
                            ComplementoGenerico(_r, "whs.DimProductosRelevantes");
                            break;

                        case "Unidad Negocio":
                            ComplementoGenerico(_r, "stg.UnidadNegocioComplemento");
                            break;

                        case "Unidad Medida":
                            ComplementoGenerico(_r, "stg.UnidadMedidaComplemento");
                            break;

                        case "Tipo Producto":
                            ComplementoGenerico(_r, "stg.TipoProductoComplemento");
                            break;

                        case "Tipo Proveedor":
                            ComplementoGenerico(_r, "stg.TipoProveedor");
                            break;

                        case "Tamano Real":
                            ComplementoGenerico(_r, "stg.TamanoRealComplemento");
                            break;

                        case "Tamano Conc":
                            ComplementoGenerico(_r, "stg.TamanoConcComplemento");
                            break;

                        case "Proveedor Retail Cegid":
                            ComplementoGenerico(_r, "stg.ProveedorRetailCegidComplemento");
                            break;

                        case "Marca":
                            ComplementoGenerico(_r, "stg.MarcaComplemento");
                            break;

                        case "Lugar Cliente":
                            ComplementoGenerico(_r, "stg.LugarClienteComplemento");
                            break;

                        case "Linea":
                            ComplementoGenerico(_r, "stg.LineaComplemento");
                            break;

                        case "GrupoItems":
                            ComplementoGenerico(_r, "stg.GrupoItemsComplemento");
                            break;

                        case "GrupoArt":
                            ComplementoGenerico(_r, "stg.GrupoArtComplemento");
                            break;

                        case "Genero":
                            ComplementoGenerico(_r, "stg.GeneroComplemento");
                            break;

                        case "Fabricante":
                            ComplementoGenerico(_r, "stg.FabricanteComplemento");
                            break;

                        case "Categoria":
                            ComplementoGenerico(_r, "stg.CategoriaComplemento");
                            break;

                        case "Almacenes":
                            ComplementoGenerico(_r, "stg.AlmacenesComplemento");
                            break;


                        case "Propio Tercero":
                            ComplementoGenerico(_r, "stg.PropioTerceroComplemento");
                            break;

                        case "Ventas Mensual":
                            VentaComplemento(_r, "stg.VentasComplemento");
                            break;

                        case "Tipos Movimientos":
                            ComplementoGenerico(_r, "stg.Tipos_Movimientos");
                            break;

                        case "Data Warehouse":
                            DataWarehouse(_r);
                            break;

                    }

                }
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
            }
            finally
            { }
        }

        private static void LibroMayor(DataRow _row)
        {
            int _id = Convert.ToInt32(_row["ID"]);
            string _complementosFolder = ConfigurationManager.AppSettings["ComplementosFolder"].ToString();
            string _fileName = string.Format("{0}{1}", _complementosFolder, _row["Archivo"]);
            Log.Write.WriteError("importando complemento " + _fileName);

            ActualizarEstadoProcess(_id, "Procesando", "");
            try
            {
                string _query = "select distinct RefDate, Origen from [Complementos$]";

                DataSet _dsExcel = ADO.Excel.getExcelDataByQuery(_fileName, _query);

                //Obtengo Perido y Origen
                foreach (DataRow _rXls in _dsExcel.Tables[0].Rows)
                {
                    if (_rXls["RefDate"].ToString() != "")
                    {
                        DateTime _Periodo = (DateTime)_rXls["RefDate"];
                        int _pAnio = _Periodo.Year;
                        int _pMonth = _Periodo.Month;
                        string _origen = _rXls["Origen"].ToString();
                        string _pYear = _Periodo.ToString();

                        string _delete = string.Format("delete [stg].[JDT1Complementos] where year(RefDate) = {0} and month(RefDate)={1} and Origen ='{2}'", _pAnio, _pMonth, _origen);
                        Log.Write.WriteError(_delete);
                        ADO.SQL.SqlExecuteNonQuery(_delete, _sqlConnection);
                    }
                }

                _dsExcel = ADO.Excel.getExcelData(_fileName);

                IList<System.Data.SqlClient.SqlBulkCopyColumnMapping> _mapping = new List<System.Data.SqlClient.SqlBulkCopyColumnMapping>();
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[0].ColumnName, "TransId")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[1].ColumnName, "Codigo")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[2].ColumnName, "LineMemo")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[3].ColumnName, "RefDate")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[4].ColumnName, "Ref1")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[5].ColumnName, "Ref2")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[6].ColumnName, "BaseRef")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[7].ColumnName, "Project")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[8].ColumnName, "OcrCode1")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[9].ColumnName, "OcrCode2")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[10].ColumnName, "OcrCode3")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[11].ColumnName, "OcrCode4")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[12].ColumnName, "OcrCode5")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[13].ColumnName, "Origen")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[14].ColumnName, "FCCurrency")));
                _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_dsExcel.Tables[0].Columns[15].ColumnName, "Saldo")));

                try
                {
                    Log.Write.WriteError("Insertando Datos");
                    ADO.SQL.SqlBulkCopyNoDelete("[stg].[JDT1Complementos]", _dsExcel.Tables[0], _mapping, _sqlConnection);
                    ActualizarEstadoProcess(_id, "Procesado OK", "");
                    Log.Write.WriteError("Datos Insertados");
                    _dsExcel.Dispose();
                }
                catch (Exception ee)
                {
                    _dsExcel.Dispose();
                    Log.Write.WriteException(ee);
                    ActualizarEstadoProcess(_id, "Error", ee.Message);
                    throw ee;
                }
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
                ActualizarEstadoProcess(_id, "Error", ex.Message);
            }
            finally { }
        }

        private static void ComplementoGenerico(DataRow _row, string tableDestino)
        {
            int _id = Convert.ToInt32(_row["ID"]);
            string _complementosFolder = ConfigurationManager.AppSettings["ComplementosFolder"].ToString();
            string _fileName = string.Format("{0}{1}", _complementosFolder, _row["Archivo"]);
            Log.Write.WriteError("importando complemento " + _fileName);

            ActualizarEstadoProcess(_id, "Procesando", "");
            try
            {


                DataSet _dsExcel = ADO.Excel.getExcelData(_fileName);

                IList<System.Data.SqlClient.SqlBulkCopyColumnMapping> _mapping = new List<System.Data.SqlClient.SqlBulkCopyColumnMapping>();
                foreach (DataColumn _c in _dsExcel.Tables[0].Columns)
                {
                    _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_c.ColumnName, _c.ColumnName)));
                }


                try
                {
                    Log.Write.WriteError("Insertando Datos");
                    ADO.SQL.SqlBulkCopy(tableDestino, _dsExcel.Tables[0], _mapping, _sqlConnection);
                    ActualizarEstadoProcess(_id, "Procesado OK", "");
                    Log.Write.WriteError("Datos Insertados");
                    _dsExcel.Dispose();
                }
                catch (Exception ee)
                {
                    _dsExcel.Dispose();
                    Log.Write.WriteException(ee);
                    ActualizarEstadoProcess(_id, "Error", ee.Message);
                    throw ee;
                }
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
                ActualizarEstadoProcess(_id, "Error", ex.Message);
            }
            finally { }
        }

        private static void VentaComplemento(DataRow _row, string tableDestino)
        {
            int _id = Convert.ToInt32(_row["ID"]);
            string _complementosFolder = ConfigurationManager.AppSettings["ComplementosFolder"].ToString();
            string _fileName = string.Format("{0}{1}", _complementosFolder, _row["Archivo"]);
            Log.Write.WriteError("importando complemento " + _fileName);

            ActualizarEstadoProcess(_id, "Procesando", "");
            try
            {
                //string _query = "select distinct Fecha_Contabilizacion, Origen from [Complementos$]";
                DataSet _dsExcel = ADO.Excel.getExcelData(_fileName);
                

                if (_dsExcel.Tables[0].Rows.Count > 0)
                {
                    DataRow _rXls = _dsExcel.Tables[0].Rows[0];
                    if (!_rXls["Fecha_Contabilizacion"].ToString().Equals(""))
                    {
                        int _PeridoAnio = DateTime.Parse(_rXls["Fecha_Contabilizacion"].ToString()).Year;
                        int _PeridoMes = DateTime.Parse(_rXls["Fecha_Contabilizacion"].ToString()).Month;
                        string _Origen = _rXls["Origen"].ToString();

                        string _detelePeriodo = string.Format("delete stg.VentasComplemento where MONTH(Fecha_Contabilizacion)={1} and year(Fecha_Contabilizacion)={0} and Origen = '{2}' ", _PeridoAnio, _PeridoMes, _Origen);
                        Log.Write.WriteError(_detelePeriodo);
                        ADO.SQL.SqlExecuteNonQuery(_detelePeriodo, _sqlConnection);
                    }

                }

                IList<System.Data.SqlClient.SqlBulkCopyColumnMapping> _mapping = new List<System.Data.SqlClient.SqlBulkCopyColumnMapping>();
                foreach (DataColumn _c in _dsExcel.Tables[0].Columns)
                {
                    _mapping.Add((new System.Data.SqlClient.SqlBulkCopyColumnMapping(_c.ColumnName, _c.ColumnName)));
                }


                try
                {
                    Log.Write.WriteError("Insertando Datos");
                    ADO.SQL.SqlBulkCopyNoDelete(tableDestino, _dsExcel.Tables[0], _mapping, _sqlConnection);

                    _dsExcel.Dispose();
                    string _deleteNull = "delete stg.VentasComplemento where Fecha_Contabilizacion is null";
                    ADO.SQL.SqlExecuteNonQuery(_deleteNull, _sqlConnection);

                    ActualizarEstadoProcess(_id, "Procesando", "Actualzando Cubo de Ventas");
                    string _exec = "exec [whs].[Sp_ActualizarFactVentas]";
                    ADO.SQL.SqlExecuteNonQuery(_exec, _sqlConnection);

                    ActualizarEstadoProcess(_id, "Procesado OK", "");
                    Log.Write.WriteError("Datos Insertados");
                }
                catch (Exception ee)
                {
                    _dsExcel.Dispose();
                    Log.Write.WriteException(ee);
                    ActualizarEstadoProcess(_id, "Error", ee.Message);
                    throw ee;
                }
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
                ActualizarEstadoProcess(_id, "Error", ex.Message);
            }
            finally { }
        }

        private static void ObjetivosTotales(DataRow _row)
        {
            int _id = Convert.ToInt32(_row["ID"]);
            string _complementosFolder = ConfigurationManager.AppSettings["ComplementosFolder"].ToString();
            string _fileName = string.Format("{0}{1}", _complementosFolder, _row["Archivo"]);
            Log.Write.WriteError("importando complemento " + _fileName);

            ActualizarEstadoProcess(_id, "Procesando", "");

            try
            {
                string _query = "select distinct AÑO, MES from [OBJETIVO TOTAL$]";

                DataSet _dsExcel = ADO.Excel.getExcelDataByQuery(_fileName, _query);

                //Obtengo Perido y Origen
                foreach (DataRow _rXls in _dsExcel.Tables[0].Rows)
                {
                    if (!_rXls["AÑO"].ToString().Equals(""))
                    {
                        string _PeridoAnio = _rXls["AÑO"].ToString();
                        string _PeridoMes = _rXls["MES"].ToString().PadLeft(2, char.Parse("0"));
                        //
                        string _detelePeriodo = string.Format("SET SQL_SAFE_UPDATES = 0; delete from rouge.objetivos_rouge where ID_YM = '{0}{1}'", _PeridoAnio, _PeridoMes);
                        Log.Write.WriteError(_detelePeriodo);
                        ADO.MySQL.MySqlExecuteNonQuery(_detelePeriodo, _mySqlCegid);
                    }

                }

                try
                {
                    _query = "select * from [OBJETIVO TOTAL$]";
                    _dsExcel = ADO.Excel.getExcelDataByQuery(_fileName, _query);

                    Log.Write.WriteError("Insertando Datos");
                    foreach (DataRow _c in _dsExcel.Tables[0].Rows)
                    {
                        if (_c["LOCAL"].ToString() != "")
                        {
                            if (_c["TOTAL  REAL"].ToString() != "")
                            {
                                string _valores = string.Format("'{0}',{2},{1},'{1}-{3}-01','{1}{3}'", _c["LOCAL"].ToString(), _c["AÑO"].ToString(), _c["TOTAL  REAL"].ToString(), _c["MES"].ToString());
                                string _queryInsert = string.Format("insert into rouge.objetivos_rouge (Sucursales,target_rouge_mensual,Year,month_year,ID_YM) values ({0})", _valores);

                                ADO.MySQL.MySqlExecuteNonQuery(_queryInsert, _mySqlCegid);
                            }
                        }
                    }
                    ActualizarEstadoProcess(_id, "Procesado OK", "");
                    Log.Write.WriteError("Datos Insertados");
                    _dsExcel.Dispose();
                }
                catch (Exception ee)
                {
                    _dsExcel.Dispose();
                    Log.Write.WriteException(ee);
                    ActualizarEstadoProcess(_id, "Error", ee.Message);
                    throw ee;
                }
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
                ActualizarEstadoProcess(_id, "Error", ex.Message);
            }
            finally { }
        }

        private static void ObjetivosDiffupar(DataRow _row)
        {
            int _id = Convert.ToInt32(_row["ID"]);
            string _complementosFolder = ConfigurationManager.AppSettings["ComplementosFolder"].ToString();
            string _fileName = string.Format("{0}{1}", _complementosFolder, _row["Archivo"]);
            Log.Write.WriteError("importando complemento " + _fileName);

            ActualizarEstadoProcess(_id, "Procesando", "");

            try
            {
                string _query = "select distinct AÑO, MES from [OBJETIVO TOTAL$]";

                DataSet _dsExcel = ADO.Excel.getExcelDataByQuery(_fileName, _query);

                //Obtengo Perido y Origen
                foreach (DataRow _rXls in _dsExcel.Tables[0].Rows)
                {
                    if (!_rXls["AÑO"].ToString().Equals(""))
                    {
                        string _PeridoAnio = _rXls["AÑO"].ToString();
                        string _PeridoMes = _rXls["MES"].ToString().PadLeft(2, char.Parse("0"));
                        //
                        string _detelePeriodo = string.Format("SET SQL_SAFE_UPDATES = 0; delete from rouge.objetivos_diffupar_rouge where ID_YM_target_rouge = '{0}{1}'", _PeridoAnio, _PeridoMes);
                        Log.Write.WriteError(_detelePeriodo);
                        ADO.MySQL.MySqlExecuteNonQuery(_detelePeriodo, _mySqlCegid);
                    }

                }
                int _line = 0;
                try
                {
                    _query = "select * from [OBJETIVO TOTAL$]";
                    _dsExcel = ADO.Excel.getExcelDataByQuery(_fileName, _query);

                    Log.Write.WriteError("Insertando Datos");
                    foreach (DataRow _c in _dsExcel.Tables[0].Rows)
                    {
                        _line = _line + 1;
                        if (_c["LOCAL"].ToString() != "")
                        {
                            if (_c["TOTAL DIFF REAL"].ToString() != "")
                            {
                                string _valores = string.Format("'{0}','{1}{3}','{1}',{2}", _c["LOCAL"].ToString(), _c["AÑO"].ToString(), _c["TOTAL DIFF REAL"].ToString().Replace(",","."), _c["MES"].ToString());
                                string _queryInsert = string.Format("insert into rouge.objetivos_diffupar_rouge (Sucursales,ID_YM_target_rouge,Year_target_Diffupar,target_diffupar_mensual) values ({0})", _valores);

                                Log.Write.WriteError(($"Query Insert Objetivos Diffupar: {_queryInsert }"));
                                ADO.MySQL.MySqlExecuteNonQuery(_queryInsert, _mySqlCegid);
                            }

                        }
                    }
                    ActualizarEstadoProcess(_id, "Procesado OK", "");
                    Log.Write.WriteError("Datos Insertados");
                    _dsExcel.Dispose();
                }
                catch (Exception ee)
                {
                    _dsExcel.Dispose();
                    Log.Write.WriteException(ee);
                    ActualizarEstadoProcess(_id, "Error", "Linea " + _line.ToString() + " " + ee.Message);
                    throw ee;
                }
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
                ActualizarEstadoProcess(_id, "Error", ex.Message);
            }
            finally { }
        }

        private static void DataWarehouse(DataRow _row)
        {
            int _id = Convert.ToInt32(_row["ID"]);
            string _estado = _row["Estado"].ToString();
            DateTime _newSchedule = DateTime.Parse(_row["FechaCarga"].ToString());
            if (_newSchedule < DateTime.Now)
            {
                Log.Write.WriteError("Actualizando Data Warehouse");

                ActualizarEstadoProcess(_id, "Procesando", "");

                try
                {

                    DateTime _start = DateTime.Now;
                    DateTime _end = DateTime.Now;
                    string _query = "select id, name from dbSchedule where status = 1 order by orden";
                    try
                    {
                        foreach (DataRow r in ADO.SQL.SqlExecuteQueryDataSet(_query, _sqlConnection).Tables[0].Rows)
                        {
                            int _scheduleID = int.Parse(r[0].ToString());
                            string _scheduleName = r[1].ToString();

                            Log.Write.WriteError(string.Format("Ejecutando Schedule: {0}", _scheduleName.ToUpper()));
                            ActualizarEstadoProcess(_id, "Procesando", _scheduleName);

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
                        }


                        Log.Write.WriteError("Actualiando Warehouse");
                        ADO.SQL.SqlExecuteNonQuery("exec [stg].[Sp_ImportDimensiones]", _sqlConnection);

                        ActualizarEstadoProcess(_id, "Procesado OK", "");
                        Log.Write.WriteError("Data Warehouse actualizado");

                        //Genera nuevo proceso                        
                        string _qUpdateProcess = string.Format("exec [dbo].[InsertProcessDW] {0}", _id);
                        ADO.SQL.SqlExecuteNonQuery(_qUpdateProcess, _sqlConnection);

                    }
                    catch (Exception ee)
                    {
                        Log.Write.WriteException(ee);
                        ActualizarEstadoProcess(_id, "Error", ee.Message);
                        throw ee;
                    }
                    finally
                    {
                        _end = DateTime.Now;
                        string _metricas = string.Format("Duración del proceso:{0} minutos", _end.Subtract(_start).TotalMinutes);
                        Log.Write.WriteError(_metricas);
                        Log.Write.WriteError("Fin");
                    }

                }
                catch (Exception ex)
                {
                    Log.Write.WriteException(ex);
                    ActualizarEstadoProcess(_id, "Error", ex.Message);
                }
                finally { }
            }
        }

        private static void ActualizarEstadoProcess(int id, string estado, string detalle)
        {
            try
            {
                string _queryUpdateStatus = string.Format("update dbprocess set estado='{0}' ,fechaProcesado = getdate(), EstadoDescripcion = '{2}' where id = {1}", estado, id, detalle.Replace("'", ""));
                ADO.SQL.SqlExecuteNonQuery(_queryUpdateStatus, _sqlConnection);
            }
            catch (Exception e)
            {
                Log.Write.WriteException(e);
            }
        }
    }
}
