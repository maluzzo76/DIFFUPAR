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
using RabbitMQ.Client;
using System.Threading.Channels;
using RabbitMQ.Client.Events;
using System.Runtime.Remoting.Channels;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace Process
{
    public class Napse
    {
        string url { get { return ConfigurationManager.AppSettings["ApiNapseUrl"].ToString(); } }
        string token { get; set; }
        Guid ProcessId { get; set; }

        public static string _sqlConnection = ConfigurationManager.ConnectionStrings["dbHost"].ConnectionString;
        internal static bool _autoACK = bool.Parse(ConfigurationManager.AppSettings["RabbitautoAck"].ToString());
        private static IList<SqlBulkCopyColumnMapping> _mappingsColumns = new List<SqlBulkCopyColumnMapping>();


        public Napse()
        {
            ProcessId = Guid.NewGuid();
            token = GetToken();
        }

        #region <REST>

        /// <summary>
        /// Obtiene el token de autenticación
        /// </summary>
        /// <returns></returns>
        string GetToken()
        {
            using (var client = new HttpClient())
            {
                string _url = url + "auth/login";

                //client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.ExpectContinue = false;

                var stringContent = new StringContent("{\r\n\"clientId\": \"bridge-api\",\r\n\"clientSecret\": \"DA39A3EE5E6B4B0D3255BFEF95601890AFD80709\"\r\n}", UnicodeEncoding.UTF8, "application/json");
                var _response = client.PostAsync(_url, stringContent).Result;
                var _res = _response.Content.ReadAsStringAsync().Result;
                dynamic _r = JObject.Parse(_res);
                string _token = _r.token.Value;
                return _token;
            }
        }

        public void GetNapseRest()
        {
            ProcessId = Guid.NewGuid();
            string _query = "select * from dbo.RabbitQueue where  estado = 'OnLine' and origen ='Rest' and ProximaEjecucion <=  getdate()";
            DataTable _dt = ADO.SQL.SqlExecuteQueryDataSet(_query, _sqlConnection).Tables[0];
            

            foreach (DataRow _r in _dt.Rows)
            {
                string _queueName = _r["QueueName"].ToString();
                int _RabbieQueueId = (int)_r["Id"];
                                
                GetAllStock(_queueName , _RabbieQueueId);               
            }
        }

        /// <summary>
        /// Obtiene el stock mensual a mes cerrado
        /// </summary>
        internal void GetAllStock(string queue, int RabbiQueueId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string _url = url + queue;
                 


                    if (token != null)
                        client.DefaultRequestHeaders.Add("x-access-token", token);

                    client.Timeout = TimeSpan.FromMinutes(20);
                    var stringContent = new StringContent("{\r\n\"max\":4000,\r\n\"skip\":2\r\n}", UnicodeEncoding.UTF8, "application/json");
                    if (queue == "stockReport/status")
                    {
                        stringContent = new StringContent("{\r\n\"max\":10000,\r\n\"skip\":10\r\n}", UnicodeEncoding.UTF8, "application/json");
                    }
                    var _response = client.PostAsync(_url, stringContent).Result;
                    var _res = _response.Content.ReadAsStringAsync().Result.Replace("{\"ack\": 0, \"detail\": [", "").Replace("]\r\n}", "");


                    //Persiste Msg
                    AddMessageRabbit(_res, RabbiQueueId);
                }
                catch (Exception ex)
                {
                    LogError(ex, RabbiQueueId);
                }
                finally 
                {
                    client.Dispose();
                }

            }
        }

        /// <summary>
        /// Transforma el stock rest a entidad
        /// </summary>
        internal void TransformarMenssageStockRest()
        {
            var _res = "";

            DataTable _dtjson = new DataTable();
            _dtjson.Columns.Add("InternalCode");
            _dtjson.Columns.Add("Stock");
            _dtjson.Columns.Add("Reserved");
            _dtjson.Columns.Add("StoreCode");
            _dtjson.Columns.Add("LocationCode");
            _dtjson.Columns.Add("LocationName");
            _dtjson.Columns.Add("ItemInventoryState");
            _dtjson.Columns.Add("Fecha");


            foreach (var _json in _res.Replace("},", ";").Split(char.Parse(";")))
            {
                string json = _json.Replace("{\"ack\":0,\"detail\":[", "") + "}".Replace("]}}", "");
                json = json.Replace("]}}", "");


                Entity entity = JsonConvert.DeserializeObject<Entity>(json);

                DataRow _row = _dtjson.NewRow();

                _row[0] = entity.InternalCode;
                _row[1] = (int)entity.Detail.Stock;
                _row[2] = (int)entity.Detail.Reserved;
                _row[3] = entity.Detail.StoreCode.ToString();
                _row[4] = entity.Detail.LocationCode;
                _row[5] = entity.Detail.LocationName;
                _row[6] = entity.Detail.ItemInventoryState;
                _row[7] = DateTime.Now;

                _dtjson.Rows.Add(_row);

            }

            //Controla si el stock se actualiza varias veces el mismo día
            string _queryDelete = string.Format("delete [stg].[NapseStock] where fecha = '{0}-{1}-{2}'", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ADO.SQL.SqlExecuteNonQuery(_queryDelete, _sqlConnection);

            //Persiste el Stock
            ADO.SQL.SqlBulkCopyNoDelete("[stg].[NapseStock]", _dtjson, _mappingsColumns, _sqlConnection);
        }

        /// <summary>
        /// Persite el mensage de rabbit en la DB
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rabbiQueueId"></param>
        internal void AddMessageRabbit(string message, int rabbitQueueId)
        {
            try
            {
                message = message.Replace("'", "");
                string _query = string.Format("insert into [dbo].[RabbitMenssage] (Msg,Estado,RabbitQueue_Id,ImportDate,ProcessId) values ('{0}','New',{1}, getdate(),'{2}')", message, rabbitQueueId, ProcessId.ToString());
                ADO.SQL.SqlExecuteNonQuery(_query, _sqlConnection);

                string _queryUpd = (($"update dbo.RabbitQueue set UltimaEjecucion = getdate(), ProximaEjecucion = dateadd(day,1,ProximaEjecucion) where id = {rabbitQueueId}"));
                ADO.SQL.SqlExecuteNonQuery(_queryUpd, _sqlConnection);
            }
            catch (Exception ex)
            {
                LogError(ex, rabbitQueueId);
            }
        }

        #endregion

        /// <summary>
        /// Persiste el log de error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="rabbitQueueId"></param>
        internal void LogError(Exception ex, int rabbitQueueId)
        {
            Log.Write.WriteException(ex);
            string _queryErr = string.Format("insert into dbo.rabbitErrorLog (ProcessDate,RabbitQueueId,ErrorMessage,Excepcion,ProcessId ) values(getdate(),{0},'{1}','{2}','{3}')", rabbitQueueId, ex.Message.Replace("'","\""), ex.InnerException, ProcessId);
            ADO.SQL.SqlExecuteNonQuery(_queryErr, _sqlConnection);
        }
    }

    public class Detail
    {
        public int Stock { get; set; }
        public int Reserved { get; set; }
        public string StoreCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string ItemInventoryState { get; set; }
    }

    public class Entity
    {
        public string InternalCode { get; set; }
        public Detail Detail { get; set; }
    }

}
