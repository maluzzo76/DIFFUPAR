using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Policy;
using Microsoft.Win32.SafeHandles;
using System.Configuration;
using System.Threading;
using System.Threading.Channels;
using System.Runtime.Remoting.Channels;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using ADO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Diagnostics.SymbolStore;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Collections;

namespace Process
{
    public class NapseCast
    {
        private static string _sqlConnection = ConfigurationManager.ConnectionStrings["dbHost"].ConnectionString;
        private static IList<SqlBulkCopyColumnMapping> _mappingsColumns = new List<SqlBulkCopyColumnMapping>();

        /// <summary>
        /// Obtiene los menssajes no migrados al Data Warehouse
        /// </summary>
        public static void JsonNapseTransform()
        {
            //process cola transaction Bi2

            RabbitJsonProcess.process();

            string _qGetRabbitMessage = "select * from dbo.V_RabbitMessageProcess  order by tipo desc";

            DataTable dataTable = ADO.SQL.SqlExecuteQueryDataSet(_qGetRabbitMessage, _sqlConnection).Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                int _RabbitQueueId = (int)row["RabbitQueue_Id"];
                int _msgId = (int)row["Id"];
                string _message = row["msg"].ToString();
                string _tipo = row["tipo"].ToString();
                string _processKey = row["ProcessId"].ToString();
                string _estado = "";
                DateTime _importDate = DateTime.Parse(row["ImportDate"].ToString());

                // Actulizar estado msg a Procesando
                string _queryResult = ($"update RabbitMenssage set Estado = 'Importando' where id = {_msgId}");
                ADO.SQL.SqlExecuteNonQuery(_queryResult, _sqlConnection);

                try
                {
                    switch (_tipo.ToUpper())
                    {
                        case "INVENTORY":
                            ProcessMessageInventory(_message, _processKey);
                            break;

                        case "TRANSACTIONS":
                            ProcessMessageTransactios(_message, _processKey);
                            break;


                        case "STOCKREPORT":
                            ProcessMessageRestStoreReport(_message, _processKey, _importDate);
                            break;

                        case "STORE":
                            ProcessMessageStore(_message, _processKey);
                            break;
                    }
                    _estado = "Procesado OK";
                }
                catch (Exception ex)
                {
                    _estado = "Error";
                    LogError(ex, _RabbitQueueId, _processKey);
                }
                finally
                {
                    string _queryEstado = ($"update RabbitMenssage set Estado = '{_estado}' where id = {_msgId}");
                    ADO.SQL.SqlExecuteNonQuery(_queryEstado, _sqlConnection);
                }
            }
        }

        internal static void ProcessMessageInventory(string message, string ProcessKey)
        {
            var jObject = JObject.Parse(message);

            DataTable _dtJson = NapseDt_TransactioInvetary();
            string _jsonTransactionId = "";

            // Recorre cada Recepcion
            foreach (var _items in jObject["inventoryControlDocumentLineItem"])
            {


                _jsonTransactionId = (string)jObject["_id"];
                string _id = (string)jObject["_id"];
                DateTime _fecha = DateTime.Parse(jObject["createdAt"].ToString());
                string _TipoOperacion = (string)jObject["documentTypeCode"];
                string _NroOperacion = (string)jObject["serialFormId"];
                string _CodigoTienda = (string)jObject["sourceRetailStoreCode"];
                string _NombreTienda = "";
                string _Motivo = "";
                string _TipoDocumento = (string)jObject["documentTypeCode"];
                string _NroDocumento = (string)jObject["serialFormId"];
                string _EstadoDocumento = (string)jObject["inventoryControlDocumentStateName"];
                string _DepositoOrigen = "";
                string _DepositoDestino = "";
                string _CodigoArticulo = (string)_items["itemCode"];
                string _NombreArticulo = "";
                decimal _Cantidad = (decimal)_items["unitCount"]["numberDecimal"];
                string _CodigoVendedor = "";
                string _NombreVendedor = "";
                string _EstadoInventario = (string)jObject["inventoryControlDocumentStateName"];
                string _MessageProcessId = ProcessKey;
                string _UserName = (string)jObject["userName"];
                string _Proveedor = (string)jObject["supplierCode"];
                string _TipoAjuste = "";
                string _NroOcSap = (string)jObject["originatorNumber"];
                string _CodigoTiendaDestino = (string)jObject["destinationRetailStoreCode"];
                string _NombreDestinoTienda = "";

                try { _DepositoOrigen = (string)_items["sourceLocation"]["code"]; } catch { }
                try { _DepositoDestino = (string)_items["destinationLocation"]["code"]; } catch { };
                if (jObject["inventoryAdjustmentTypeCode"].Count() > 0) _TipoAjuste = (string)jObject["inventoryAdjustmentTypeCode"][0];


                DataRow _nRow = _dtJson.NewRow();
                //Mapping Row
                _nRow["Id"] = _id;
                _nRow["Fecha"] = _fecha;
                _nRow["TipoOperacion"] = _TipoOperacion;
                _nRow["NroOperacion"] = _NroOperacion;
                _nRow["CodigoTienda"] = _CodigoTienda;
                _nRow["NombreTienda"] = _NombreTienda;
                _nRow["Motivo"] = _Motivo;
                _nRow["TipoDocumento"] = _TipoDocumento;
                _nRow["NroDocumento"] = _NroDocumento;
                _nRow["EstadoDocumento"] = _EstadoDocumento;
                _nRow["DepositoOrigen"] = _DepositoOrigen;
                _nRow["DepositoDestino"] = _DepositoDestino;
                _nRow["CodigoArticulo"] = _CodigoArticulo;
                _nRow["NombreArticulo"] = _NombreArticulo;
                _nRow["Cantidad"] = _Cantidad;
                _nRow["CodigoVendedor"] = _CodigoVendedor;
                _nRow["NombreVendedor"] = _NombreVendedor;
                _nRow["EstadoInventario"] = _EstadoInventario;
                _nRow["MessageProcessId"] = ProcessKey;
                _nRow["UserName"] = _UserName;
                _nRow["Proveedor"] = _Proveedor;
                _nRow["TipoAjuste"] = _TipoAjuste;
                _nRow["NroOcSap"] = _NroOcSap;
                _nRow["CodigoTiendaDestino"] = _CodigoTiendaDestino;
                _nRow["NombreTiendaDestino"] = _NombreDestinoTienda;

                bool _isInserted = false;

                //Agregar los numeros de recepcion
                foreach (var _CR in jObject["inventoryControlDocumentReference"])
                {
                    DataRow _nNrRoe = _dtJson.NewRow();
                    //Mapping Row
                    _nNrRoe["Id"] = _id;
                    _nNrRoe["Fecha"] = _fecha;
                    _nNrRoe["TipoOperacion"] = _TipoOperacion;
                    _nNrRoe["NroOperacion"] = _NroOperacion;
                    _nNrRoe["CodigoTienda"] = _CodigoTienda;
                    _nNrRoe["NombreTienda"] = _NombreTienda;
                    _nNrRoe["Motivo"] = _Motivo;
                    _nNrRoe["TipoDocumento"] = _TipoDocumento;
                    _nNrRoe["NroDocumento"] = _NroDocumento;
                    _nNrRoe["EstadoDocumento"] = _EstadoDocumento;
                    _nNrRoe["DepositoOrigen"] = _DepositoOrigen;
                    _nNrRoe["DepositoDestino"] = _DepositoDestino;
                    _nNrRoe["CodigoArticulo"] = _CodigoArticulo;
                    _nNrRoe["NombreArticulo"] = _NombreArticulo;
                    _nNrRoe["Cantidad"] = _Cantidad;
                    _nNrRoe["CodigoVendedor"] = _CodigoVendedor;
                    _nNrRoe["NombreVendedor"] = _NombreVendedor;
                    _nNrRoe["EstadoInventario"] = _EstadoInventario;
                    _nNrRoe["MessageProcessId"] = ProcessKey;
                    _nNrRoe["UserName"] = _UserName;
                    _nNrRoe["Proveedor"] = _Proveedor;
                    _nNrRoe["TipoAjuste"] = _TipoAjuste;
                    _nNrRoe["NroOcSap"] = _NroOcSap;
                    _nNrRoe["CodigoRecepcion"] = (string)_CR["referToInventoryControlDocumentDocNumber"]["numberDecimal"];
                    _nNrRoe["NumeroPedidoExterno"] = (string)_CR["referToInventoryControlDocumentDocNumber"]["numberDecimal"];
                    _nNrRoe["CodigoTiendaDestino"] = _CodigoTiendaDestino;
                    _nNrRoe["NombreTiendaDestino"] = _NombreDestinoTienda;

                    _dtJson.Rows.Add(_nNrRoe);
                    _isInserted = true;
                }

                if (!_isInserted)
                    _dtJson.Rows.Add(_nRow);
            }

            string _queryEstado = ($"update RabbitMenssage set JsonTransactionID = isnull(JsonTransactionID,'') + '{_jsonTransactionId}' where ProcessId = '{ProcessKey}'");
            ADO.SQL.SqlExecuteNonQuery(_queryEstado, _sqlConnection);

            ADO.SQL.SqlBulkCopyNoDelete("[stg].[NapseTransactions]", _dtJson, _mappingsColumns, _sqlConnection);
        }

        internal static void ProcessMessageTransactios(string message, string ProcessKey)
        {

            var jObject = JObject.Parse(message);
            string _jsonTransactionId = "";
            DataTable _dtJson = NapseDt_TransactioInvetary();

            foreach (var _items in jObject["items"])
            {
                DataRow _nRow = _dtJson.NewRow();

                _jsonTransactionId = (string)jObject["_id"];
                string _id = (string)jObject["_id"];
                DateTime _fecha = DateTime.Parse(jObject["beginDateTime"].ToString());//createdAt
                string _TipoOperacion = (string)jObject["trxType"];
                string _NroOperacion = (string)jObject["trxNumber"];
                // string _CodigoTienda = ($"{(string)jObject["storeCode"]}-{(string)_items["terminalCode"]}-{(string)jObject["trxNumber"]}") ;
                string _CodigoTienda = (string)jObject["storeCode"];
                string _NombreTienda = (string)jObject["storeName"];
                string _Motivo = "";
                string _TipoDocumento = (string)_items["billType"];
                string _NroDocumento = (string)_items["billNumber"];
                string _EstadoDocumento = "";
                string _DepositoOrigen = (string)_items["locationCode"];
                string _DepositoDestino = "";
                string _CodigoArticulo = (string)_items["internalCode"];
                string _NombreArticulo = (string)_items["description"];
                decimal _Cantidad = (decimal)_items["quantity"];
                string _CodigoVendedor = (string)_items["sellerID"];
                string _NombreVendedor = (string)_items["sellerName"];
                string _EstadoInventario = "";
                string _MessageProcessId = ProcessKey;
                string _CodigoTerminal = (string)_items["terminalCode"];
                string _CodigoExterno = ($"{_CodigoTienda}{_CodigoTerminal}{_NroOperacion}");
                string _PuntoDeVenta = (string)jObject["fiscalPosNumber"];
                string _LetraComprobante = (string)jObject["serieOfficialBill"];
                string _Canal = (string)jObject["channel"];
                string _ClienteCodigo = (string)jObject["partyIdentificationNumber"];
                string _ClienteNombre = (string)jObject["partyFirstName"];
                string _ClienteApellido = (string)jObject["partyLastName"];
                decimal _PrecioUnitarioSinIva = (decimal)_items["unitPriceWithDiscounts"];
                decimal _PrecioUnitarioConIva = (decimal)_items["unitPrice"];
                decimal _PrecioTotalSinIva = (decimal)_items["netAmount"];
                decimal _PrecioTotalConIva = (decimal)_items["customerExtendedAmount"];
                decimal _DescuentoManual = (decimal)_items["manualDiscountAmount"];
                decimal _DescuentoPromo = (decimal)_items["promoDiscountAmount"];
                int _voidedQuantity = (int)_items["voidedQuantity"];

                //valido la cancelacion
                bool _voiding = (bool)_items["voiding"];
                bool _voidFlag = (bool)_items["voidFlag"];


                if (!_voiding && _voidFlag)
                {
                    _Cantidad = 0;
                    _PrecioUnitarioSinIva = 0;
                    _PrecioUnitarioConIva = 0;
                    _PrecioTotalSinIva = 0;
                    _PrecioTotalConIva = 0;
                }

                if (_voiding && !_voidFlag)
                {
                    _Cantidad = 0;
                    _PrecioUnitarioSinIva = 0;
                    _PrecioUnitarioConIva = 0;
                    _PrecioTotalSinIva = 0;
                    _PrecioTotalConIva = 0;
                }



                if (_voidedQuantity > 0)
                {
                    _Cantidad = _Cantidad * -1;
                    _PrecioUnitarioSinIva = _PrecioUnitarioSinIva * -1;
                    _PrecioUnitarioConIva = _PrecioUnitarioConIva * -1;
                    _PrecioTotalSinIva = _PrecioTotalSinIva * -1;
                    _PrecioTotalConIva = _PrecioTotalConIva * -1;
                }



                _nRow["Id"] = _id;
                _nRow["Fecha"] = _fecha;
                _nRow["TipoOperacion"] = _TipoOperacion;
                _nRow["NroOperacion"] = _NroOperacion;
                _nRow["CodigoTienda"] = _CodigoTienda;
                _nRow["NombreTienda"] = _NombreTienda;
                _nRow["Motivo"] = _Motivo;
                _nRow["TipoDocumento"] = _TipoDocumento;
                _nRow["NroDocumento"] = _NroDocumento;
                _nRow["EstadoDocumento"] = _EstadoDocumento;
                _nRow["DepositoOrigen"] = _DepositoOrigen;
                _nRow["DepositoDestino"] = _DepositoDestino;
                _nRow["CodigoArticulo"] = _CodigoArticulo;
                _nRow["NombreArticulo"] = _NombreArticulo;

                _nRow["CodigoVendedor"] = _CodigoVendedor;
                _nRow["NombreVendedor"] = _NombreVendedor;
                _nRow["EstadoInventario"] = _EstadoInventario;
                _nRow["MessageProcessId"] = ProcessKey;
                _nRow["CodigoExterno"] = _CodigoExterno;
                _nRow["PuntoDeVenta"] = _PuntoDeVenta;
                _nRow["LetraComprobante"] = _LetraComprobante;
                _nRow["Canal"] = _Canal;
                _nRow["ClienteCodigo"] = _ClienteCodigo;
                _nRow["ClienteNombre"] = _ClienteNombre;
                _nRow["ClienteApellido"] = _ClienteApellido;


                _nRow["Cantidad"] = _Cantidad;
                _nRow["PrecioUnitarioSinIva"] = _PrecioUnitarioSinIva;
                _nRow["PrecioInitarioTotal"] = _PrecioUnitarioConIva;
                _nRow["PrecioTotalSinIva"] = _PrecioTotalSinIva;
                _nRow["PrecioTotal"] = _PrecioTotalConIva;
                _nRow["DescuentoPromo"] = _DescuentoPromo;
                _nRow["DescuentoManual"] = _DescuentoManual;



                bool _inserted = false;

                if (_items["itemTicketStockInformationList"] != null && _items["itemTicketStockInformationList"].ToList().Count > 0)
                {
                    foreach (var _item1 in _items["itemTicketStockInformationList"])
                    {
                        DataRow _ninf = _dtJson.NewRow();
                        _EstadoInventario = (string)_item1["itemInventoryStateCode"];




                        _ninf["Id"] = _id;
                        _ninf["Fecha"] = _fecha;
                        _ninf["TipoOperacion"] = _TipoOperacion;
                        _ninf["NroOperacion"] = _NroOperacion;
                        _ninf["CodigoTienda"] = _CodigoTienda;
                        _ninf["NombreTienda"] = _NombreTienda;
                        _ninf["Motivo"] = _Motivo;
                        _ninf["TipoDocumento"] = _TipoDocumento;
                        _ninf["NroDocumento"] = _NroDocumento;
                        _ninf["EstadoDocumento"] = _EstadoDocumento;
                        _ninf["DepositoOrigen"] = _DepositoOrigen;
                        _ninf["DepositoDestino"] = _DepositoDestino;
                        _ninf["CodigoArticulo"] = _CodigoArticulo;
                        _ninf["NombreArticulo"] = _NombreArticulo;
                        _ninf["Cantidad"] = _Cantidad;
                        _ninf["CodigoVendedor"] = _CodigoVendedor;
                        _ninf["NombreVendedor"] = _NombreVendedor;
                        _ninf["EstadoInventario"] = _EstadoInventario;
                        _ninf["MessageProcessId"] = ProcessKey;
                        _ninf["CodigoExterno"] = _CodigoExterno;
                        _ninf["PuntoDeVenta"] = _PuntoDeVenta;
                        _ninf["LetraComprobante"] = _LetraComprobante;
                        _ninf["Canal"] = _Canal;
                        _ninf["ClienteCodigo"] = _ClienteCodigo;
                        _ninf["ClienteNombre"] = _ClienteNombre;
                        _ninf["ClienteApellido"] = _ClienteApellido;
                        _ninf["PrecioUnitarioSinIva"] = _PrecioUnitarioSinIva;
                        _ninf["PrecioInitarioTotal"] = _PrecioUnitarioConIva;
                        _ninf["PrecioTotalSinIva"] = _PrecioTotalSinIva;
                        _ninf["PrecioTotal"] = _PrecioTotalConIva;
                        _ninf["DescuentoPromo"] = _DescuentoPromo;
                        _ninf["DescuentoManual"] = _DescuentoManual;

                        _dtJson.Rows.Add(_ninf);

                        _inserted = true;
                    }
                }

                if (!_inserted)
                    _dtJson.Rows.Add(_nRow);


            }
            string _queryEstado = ($"update RabbitMenssage set JsonTransactionID = isnull(JsonTransactionID,'') + '{_jsonTransactionId}' where ProcessId = '{ProcessKey}'");
            ADO.SQL.SqlExecuteNonQuery(_queryEstado, _sqlConnection);

            ADO.SQL.SqlBulkCopyNoDelete("[stg].[NapseTransactions]", _dtJson, _mappingsColumns, _sqlConnection);
        }

        internal static void ProcessMessageStore(string message, string ProcessKey)
        {
            var jObject = JObject.Parse(message);

            DataTable _dtJson = NapseDt_Store();
            string _jsonTransactionId = "";

            foreach (var _items in jObject["data"])
            {
                DataRow _nRow = _dtJson.NewRow();

                _nRow["Codigo"] = (string)_items["code"];
                _nRow["Nombre"] = (string)_items["name"];
                _nRow["TiendaDigital"] = (string)_items["digitalStore"];

                _dtJson.Rows.Add(_nRow);
            }

            string _queryEstado = ($"update RabbitMenssage set JsonTransactionID = isnull(JsonTransactionID,'') + '{_jsonTransactionId}' where ProcessId = '{ProcessKey}'");
            ADO.SQL.SqlExecuteNonQuery(_queryEstado, _sqlConnection);

            ADO.SQL.SqlBulkCopy("[whs].[DimTiendaRetail]", _dtJson, _mappingsColumns, _sqlConnection);
        }

        static void LogError(Exception ex, int rabbitQueueId, string ProcessId)
        {
            Log.Write.WriteException(ex);
            string _queryErr = string.Format("insert into dbo.rabbitErrorLog (ProcessDate,RabbitQueueId,ErrorMessage,Excepcion,ProcessId ) values(getdate(),{0},'{1}','{2}','{3}')", rabbitQueueId, ex.Message, ex.ToString(), ProcessId);
            ADO.SQL.SqlExecuteNonQuery(_queryErr, _sqlConnection);
        }

        internal static void ProcessMessageRestStoreReport(string message, string ProcessKey, DateTime ImportDate)
        {
            var jObject = JObject.Parse(message);

            DataTable _dtJson = NapseDt_TransactioInvetary();
            string _jsonTransactionId = "";

            foreach (var _items in jObject["detail"])
            {
                DataRow _nRow = _dtJson.NewRow();

                _jsonTransactionId = ProcessKey;

                _nRow["Id"] = ProcessKey;
                _nRow["Fecha"] = ImportDate;
                _nRow["TipoOperacion"] = "StockReport";
                _nRow["NroOperacion"] = "";
                _nRow["CodigoTienda"] = (string)_items["detail"]["storeCode"];
                _nRow["NombreTienda"] = (string)_items["detail"]["locationCode"];
                _nRow["Motivo"] = "";
                _nRow["TipoDocumento"] = "";
                _nRow["NroDocumento"] = "";
                _nRow["EstadoDocumento"] = "";
                _nRow["DepositoOrigen"] = (string)_items["detail"]["locationCode"];
                _nRow["DepositoOrigenNombre"] = (string)_items["detail"]["locationName"];
                _nRow["DepositoDestino"] = "";
                _nRow["CodigoArticulo"] = (string)_items["internalCode"];
                _nRow["NombreArticulo"] = "";
                _nRow["Cantidad"] = (decimal)_items["detail"]["stock"];
                _nRow["CodigoVendedor"] = "";
                _nRow["NombreVendedor"] = "";
                _nRow["EstadoInventario"] = (string)_items["detail"]["itemInventoryState"];
                _nRow["MessageProcessId"] = ProcessKey;


                _dtJson.Rows.Add(_nRow);
            }
            string _queryEstado = ($"update RabbitMenssage set JsonTransactionID = isnull(JsonTransactionID,'') + '{_jsonTransactionId}' where ProcessId = '{ProcessKey}'");
            ADO.SQL.SqlExecuteNonQuery(_queryEstado, _sqlConnection);



            ADO.SQL.SqlBulkCopyNoDelete("[stg].[NapseTransactions]", _dtJson, _mappingsColumns, _sqlConnection);
        }

        internal static DataTable NapseDt_TransactioInvetary()
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("Id", typeof(string));
            _dt.Columns.Add("Fecha", typeof(DateTime));
            _dt.Columns.Add("TipoOperacion", typeof(string));
            _dt.Columns.Add("NroOperacion", typeof(string));
            _dt.Columns.Add("CodigoTienda", typeof(string));
            _dt.Columns.Add("NombreTienda", typeof(string));
            _dt.Columns.Add("Motivo", typeof(string));
            _dt.Columns.Add("TipoDocumento", typeof(string));
            _dt.Columns.Add("NroDocumento", typeof(string));
            _dt.Columns.Add("EstadoDocumento", typeof(string));
            _dt.Columns.Add("DepositoOrigen", typeof(string));
            _dt.Columns.Add("DepositoOrigenNombre", typeof(string));
            _dt.Columns.Add("DepositoDestino", typeof(string));
            _dt.Columns.Add("CodigoArticulo", typeof(string));
            _dt.Columns.Add("NombreArticulo", typeof(string));
            _dt.Columns.Add("Cantidad", typeof(decimal));
            _dt.Columns.Add("CodigoVendedor", typeof(string));
            _dt.Columns.Add("NombreVendedor", typeof(string));
            _dt.Columns.Add("EstadoInventario", typeof(string));
            _dt.Columns.Add("MessageProcessId", typeof(string));
            _dt.Columns.Add("UserName", typeof(string));
            _dt.Columns.Add("Proveedor", typeof(string));
            _dt.Columns.Add("TipoAjuste", typeof(string));
            //------------------------------------------------
            _dt.Columns.Add("CodigoRecepcion", typeof(string));
            _dt.Columns.Add("NroOcSap", typeof(string));
            _dt.Columns.Add("NumeroPedidoExterno", typeof(string));
            _dt.Columns.Add("CodigoTiendaDestino", typeof(string));
            _dt.Columns.Add("NombreTiendaDestino", typeof(string));
            //------------------------------------------------
            _dt.Columns.Add("CodigoExterno", typeof(string));
            _dt.Columns.Add("PuntoDeVenta", typeof(string));
            _dt.Columns.Add("LetraComprobante", typeof(string));
            _dt.Columns.Add("PrecioUnitarioSinIva", typeof(decimal));
            _dt.Columns.Add("PrecioInitarioTotal", typeof(decimal));
            _dt.Columns.Add("PrecioTotalSinIva", typeof(decimal));
            _dt.Columns.Add("PrecioTotal", typeof(decimal));
            _dt.Columns.Add("DescuentoPromo", typeof(decimal));
            _dt.Columns.Add("DescuentoManual", typeof(decimal));
            _dt.Columns.Add("Canal", typeof(string));
            _dt.Columns.Add("ClienteCodigo", typeof(string));
            _dt.Columns.Add("ClienteNombre", typeof(string));
            _dt.Columns.Add("ClienteApellido", typeof(string));





            return _dt;
        }


        internal static DataTable NapseDt_Store()
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("Codigo", typeof(string));
            _dt.Columns.Add("Nombre", typeof(string));
            _dt.Columns.Add("TiendaDigital", typeof(string));

            return _dt;
        }
    }

    public class RabbitJsonProcess
    {
        private static string _sqlConnection = ConfigurationManager.ConnectionStrings["dbHost"].ConnectionString;
        private static IList<SqlBulkCopyColumnMapping> _mappingsColumns = new List<SqlBulkCopyColumnMapping>();
        static IList<string> _transacciones = new List<string>();


        public static void process()
        {
            Log.Write.WriteError("Inicio Proceso jsonFiles");
            _sqlConnection = ConfigurationManager.ConnectionStrings["dbHost"].ConnectionString;
            _transacciones.Add($"trxType,storeCode,beginDateTime,Id,total");

            string _directoryRoot = ConfigurationManager.AppSettings["ProcessJsonPath"].ToString();
            string _DirectoryPath = $"{_directoryRoot}\\pendientes";
            string _DirectoryErrorPath = $"{_directoryRoot}\\noProcesados";
            string _DirectoryProcesadosPath = $"{_directoryRoot}\\procesados";

            //creo los directorios si no existen
            if (!Directory.Exists(_directoryRoot))
                Directory.CreateDirectory(_directoryRoot);

            if (!Directory.Exists(_DirectoryPath))
                Directory.CreateDirectory(_DirectoryPath);

            if (!Directory.Exists(_DirectoryErrorPath))
                Directory.CreateDirectory(_DirectoryErrorPath);

            if (!Directory.Exists(_DirectoryProcesadosPath))
                Directory.CreateDirectory(_DirectoryProcesadosPath);


            int _count = 0;
            //Console.WriteLine($" [*] Mensages procesados '{_count}");


            foreach (string filepath in System.IO.Directory.GetFiles(_DirectoryPath))
            {
                FileInfo _fInfo = new FileInfo(filepath);

                try
                {
                    if (File.Exists(filepath)) // Verifica si el archivo existe
                    {
                        string jsonContent = File.ReadAllText(filepath);

                        ProcessMessageTransactios(jsonContent, filepath);

                        _count++;

                        Console.WriteLine($" [*] Mensages procesados '{_count}");
                    }
                    else
                    {
                        Log.Write.WriteError($"El archivo ({filepath}) no existe.");
                        Console.WriteLine("El archivo no existe.");
                    }

                    File.Move(filepath, $"{_DirectoryProcesadosPath}\\{_fInfo.Name}");
                }
                catch (Exception ex)
                {
                    Log.Write.WriteException(ex);
                }
            }
            File.WriteAllLines($"{_directoryRoot}\\{DateTime.Now.ToString().Replace("/", "").Replace(":", "").Trim()}_transacciones_log.csv", _transacciones);

            Log.Write.WriteError("Inicio Proceso jsonFiles");
        }

        static void ProcessMessageTransactios(string message, string ProcessKey)
        {
            var jObject = JObject.Parse(message);
            string _jsonTransactionId = "";
            DataTable _dtJson = NapseDt_TransactioInvetary();

            foreach (var _items in jObject["items"])
            {
                DataRow _nRow = _dtJson.NewRow();

                _jsonTransactionId = (string)jObject["_id"];
                string _id = (string)jObject["_id"];
                DateTime _fecha = DateTime.Parse(jObject["createdAt"].ToString());//createdAt
                string _TipoOperacion = (string)jObject["trxType"];
                string _NroOperacion = (string)jObject["trxNumber"];
                // string _CodigoTienda = ($"{(string)jObject["storeCode"]}-{(string)_items["terminalCode"]}-{(string)jObject["trxNumber"]}") ;
                string _CodigoTienda = (string)jObject["storeCode"];
                string _NombreTienda = (string)jObject["storeName"];
                string _Motivo = "";
                string _TipoDocumento = (string)_items["billType"];
                string _NroDocumento = (string)_items["billNumber"];
                string _EstadoDocumento = "";
                string _DepositoOrigen = (string)_items["locationCode"];
                string _DepositoDestino = "";
                string _CodigoArticulo = (string)_items["internalCode"];
                string _NombreArticulo = (string)_items["description"];
                decimal _Cantidad = (decimal)_items["quantity"];
                string _CodigoVendedor = (string)_items["sellerID"];
                string _NombreVendedor = (string)_items["sellerName"];
                string _EstadoInventario = "";
                string _MessageProcessId = ProcessKey;
                string _CodigoTerminal = (string)_items["terminalCode"];
                string _CodigoExterno = ($"{_CodigoTienda}{_CodigoTerminal}{_NroOperacion}");
                string _PuntoDeVenta = (string)jObject["fiscalPosNumber"];
                string _LetraComprobante = (string)jObject["serieOfficialBill"];
                string _Canal = (string)jObject["channel"];
                string _ClienteCodigo = (string)jObject["partyIdentificationNumber"];
                string _ClienteNombre = (string)jObject["partyFirstName"];
                string _ClienteApellido = (string)jObject["partyLastName"];
                decimal _PrecioUnitarioSinIva = (decimal)_items["unitPriceWithDiscounts"];
                decimal _PrecioUnitarioConIva = (decimal)_items["unitPrice"];
                decimal _PrecioTotalSinIva = (decimal)_items["netAmount"];
                decimal _PrecioTotalConIva = (decimal)_items["customerExtendedAmount"];
                decimal _DescuentoManual = (decimal)_items["manualDiscountAmount"];
                decimal _DescuentoPromo = (decimal)_items["promoDiscountAmount"];
                int _voidedQuantity = (int)_items["voidedQuantity"];
                bool _voiding = (bool)_items["voiding"];
                bool _voidFlag = (bool)_items["voidFlag"];
                bool _returned = (bool)_items["returned"];

                //aplico descuentos 
               

                bool _isAnulado = false;
                //Anula el producto

                if (_TipoOperacion == "Return")
                {
                    if (!_voiding && !_voidFlag)
                    {                       
                        _Cantidad = _Cantidad * -1;
                        _PrecioTotalSinIva = _PrecioTotalSinIva * -1;
                        _PrecioTotalConIva = _PrecioTotalConIva * -1;
                    }
                    else if (!_voiding && _voidFlag)
                    {                       
                        _Cantidad = _Cantidad * -1;
                        _PrecioTotalSinIva = _PrecioTotalSinIva * -1;
                        _PrecioTotalConIva = _PrecioTotalConIva * -1;
                    }

                }
                else
                {
                    if (_voiding == false && _voidFlag == true)
                    {
                        _isAnulado = true;
                        _Cantidad = _Cantidad * -1;
                        _PrecioTotalSinIva = _PrecioTotalSinIva * -1;
                        _PrecioTotalConIva = _PrecioTotalConIva * -1;
                    }

                    if (_voiding == true && _voidFlag == false)
                    {
                        _isAnulado = true;
                        _Cantidad = _Cantidad * -1;
                        _PrecioTotalSinIva = _PrecioTotalSinIva * -1;
                        _PrecioTotalConIva = _PrecioTotalConIva * -1;
                    }


                    if (_voiding == false && _voidFlag == false && _voidedQuantity > 0)
                    {                        
                        _Cantidad = _Cantidad * -1;
                        _PrecioTotalSinIva = _PrecioTotalSinIva * -1;
                        _PrecioTotalConIva = _PrecioTotalConIva * -1;
                    }
                }

                if (_isAnulado == false)
                {

                    _nRow["Id"] = _id;
                    _nRow["Fecha"] = _fecha;
                    _nRow["TipoOperacion"] = _TipoOperacion;
                    _nRow["NroOperacion"] = _NroOperacion;
                    _nRow["CodigoTienda"] = _CodigoTienda;
                    _nRow["NombreTienda"] = _NombreTienda;
                    _nRow["Motivo"] = _Motivo;
                    _nRow["TipoDocumento"] = _TipoDocumento;
                    _nRow["NroDocumento"] = _NroDocumento;
                    _nRow["EstadoDocumento"] = _EstadoDocumento;
                    _nRow["DepositoOrigen"] = _DepositoOrigen;
                    _nRow["DepositoDestino"] = _DepositoDestino;
                    _nRow["CodigoArticulo"] = _CodigoArticulo;
                    _nRow["NombreArticulo"] = _NombreArticulo;

                    _nRow["CodigoVendedor"] = _CodigoVendedor;
                    _nRow["NombreVendedor"] = _NombreVendedor;
                    _nRow["EstadoInventario"] = _EstadoInventario;
                    _nRow["MessageProcessId"] = ProcessKey;
                    _nRow["CodigoExterno"] = _CodigoExterno;
                    _nRow["PuntoDeVenta"] = _PuntoDeVenta;
                    _nRow["LetraComprobante"] = _LetraComprobante;
                    _nRow["Canal"] = _Canal;
                    _nRow["ClienteCodigo"] = _ClienteCodigo;
                    _nRow["ClienteNombre"] = _ClienteNombre;
                    _nRow["ClienteApellido"] = _ClienteApellido;


                    _nRow["Cantidad"] = _Cantidad;
                    _nRow["PrecioUnitarioSinIva"] = _PrecioUnitarioSinIva;
                    _nRow["PrecioInitarioTotal"] = _PrecioUnitarioConIva;
                    _nRow["PrecioTotalSinIva"] = _PrecioTotalSinIva;
                    _nRow["PrecioTotal"] = _PrecioTotalConIva;
                    _nRow["DescuentoPromo"] = _DescuentoPromo;
                    _nRow["DescuentoManual"] = _DescuentoManual;

                    string _log = $"{_NroOperacion};{(string)jObject["storeCode"]};{_fecha},{_id};{_PrecioTotalConIva}";
                    _transacciones.Add(_log);

                    bool _inserted = false;

                    if (_items["itemTicketStockInformationList"] != null && _items["itemTicketStockInformationList"].ToList().Count > 0)
                    {
                        foreach (var _item1 in _items["itemTicketStockInformationList"])
                        {
                            DataRow _ninf = _dtJson.NewRow();
                            _EstadoInventario = (string)_item1["itemInventoryStateCode"];

                            _ninf["Id"] = _id;
                            _ninf["Fecha"] = _fecha;
                            _ninf["TipoOperacion"] = _TipoOperacion;
                            _ninf["NroOperacion"] = _NroOperacion;
                            _ninf["CodigoTienda"] = _CodigoTienda;
                            _ninf["NombreTienda"] = _NombreTienda;
                            _ninf["Motivo"] = _Motivo;
                            _ninf["TipoDocumento"] = _TipoDocumento;
                            _ninf["NroDocumento"] = _NroDocumento;
                            _ninf["EstadoDocumento"] = _EstadoDocumento;
                            _ninf["DepositoOrigen"] = _DepositoOrigen;
                            _ninf["DepositoDestino"] = _DepositoDestino;
                            _ninf["CodigoArticulo"] = _CodigoArticulo;
                            _ninf["NombreArticulo"] = _NombreArticulo;
                            _ninf["Cantidad"] = _Cantidad;
                            _ninf["CodigoVendedor"] = _CodigoVendedor;
                            _ninf["NombreVendedor"] = _NombreVendedor;
                            _ninf["EstadoInventario"] = _EstadoInventario;
                            _ninf["MessageProcessId"] = ProcessKey;
                            _ninf["CodigoExterno"] = _CodigoExterno;
                            _ninf["PuntoDeVenta"] = _PuntoDeVenta;
                            _ninf["LetraComprobante"] = _LetraComprobante;
                            _ninf["Canal"] = _Canal;
                            _ninf["ClienteCodigo"] = _ClienteCodigo;
                            _ninf["ClienteNombre"] = _ClienteNombre;
                            _ninf["ClienteApellido"] = _ClienteApellido;
                            _ninf["PrecioUnitarioSinIva"] = _PrecioUnitarioSinIva;
                            _ninf["PrecioInitarioTotal"] = _PrecioUnitarioConIva;
                            _ninf["PrecioTotalSinIva"] = _PrecioTotalSinIva;
                            _ninf["PrecioTotal"] = _PrecioTotalConIva;
                            _ninf["DescuentoPromo"] = _DescuentoPromo;
                            _ninf["DescuentoManual"] = _DescuentoManual;

                            _dtJson.Rows.Add(_ninf);

                            _inserted = true;
                        }
                    }

                    if (!_inserted)
                        _dtJson.Rows.Add(_nRow);
                }
            }

            ADO.SQL.SqlBulkCopyNoDelete("[stg].[NapseTransactions]", _dtJson, _mappingsColumns, _sqlConnection);

        }

        internal static DataTable NapseDt_TransactioInvetary()
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("Id", typeof(string));
            _dt.Columns.Add("Fecha", typeof(DateTime));
            _dt.Columns.Add("TipoOperacion", typeof(string));
            _dt.Columns.Add("NroOperacion", typeof(string));
            _dt.Columns.Add("CodigoTienda", typeof(string));
            _dt.Columns.Add("NombreTienda", typeof(string));
            _dt.Columns.Add("Motivo", typeof(string));
            _dt.Columns.Add("TipoDocumento", typeof(string));
            _dt.Columns.Add("NroDocumento", typeof(string));
            _dt.Columns.Add("EstadoDocumento", typeof(string));
            _dt.Columns.Add("DepositoOrigen", typeof(string));
            _dt.Columns.Add("DepositoOrigenNombre", typeof(string));
            _dt.Columns.Add("DepositoDestino", typeof(string));
            _dt.Columns.Add("CodigoArticulo", typeof(string));
            _dt.Columns.Add("NombreArticulo", typeof(string));
            _dt.Columns.Add("Cantidad", typeof(decimal));
            _dt.Columns.Add("CodigoVendedor", typeof(string));
            _dt.Columns.Add("NombreVendedor", typeof(string));
            _dt.Columns.Add("EstadoInventario", typeof(string));
            _dt.Columns.Add("MessageProcessId", typeof(string));
            _dt.Columns.Add("UserName", typeof(string));
            _dt.Columns.Add("Proveedor", typeof(string));
            _dt.Columns.Add("TipoAjuste", typeof(string));
            //------------------------------------------------
            _dt.Columns.Add("CodigoRecepcion", typeof(string));
            _dt.Columns.Add("NroOcSap", typeof(string));
            _dt.Columns.Add("NumeroPedidoExterno", typeof(string));
            _dt.Columns.Add("CodigoTiendaDestino", typeof(string));
            _dt.Columns.Add("NombreTiendaDestino", typeof(string));
            //------------------------------------------------
            _dt.Columns.Add("CodigoExterno", typeof(string));
            _dt.Columns.Add("PuntoDeVenta", typeof(string));
            _dt.Columns.Add("LetraComprobante", typeof(string));
            _dt.Columns.Add("PrecioUnitarioSinIva", typeof(decimal));
            _dt.Columns.Add("PrecioInitarioTotal", typeof(decimal));
            _dt.Columns.Add("PrecioTotalSinIva", typeof(decimal));
            _dt.Columns.Add("PrecioTotal", typeof(decimal));
            _dt.Columns.Add("DescuentoPromo", typeof(decimal));
            _dt.Columns.Add("DescuentoManual", typeof(decimal));
            _dt.Columns.Add("Canal", typeof(string));
            _dt.Columns.Add("ClienteCodigo", typeof(string));
            _dt.Columns.Add("ClienteNombre", typeof(string));
            _dt.Columns.Add("ClienteApellido", typeof(string));





            return _dt;
        }
    }

}
