using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.CompilerServices;
using System.Collections;
using System.IO;

namespace NapseServices
{
    internal class Program
    {


        static string token { get; set; }
        static Guid ProcessId { get; set; }

        public static string _sqlConnection = ConfigurationManager.ConnectionStrings["dbHost"].ConnectionString;
        internal static string _rabbitUrl = ConfigurationManager.AppSettings["ApiRabbitUrl"];
        internal static string _rabbitUser = ConfigurationManager.AppSettings["ApiRabbitUser"];
        internal static string _rabbitPass = ConfigurationManager.AppSettings["ApiRabbitPassword"];
        internal static bool _autoACK = bool.Parse(ConfigurationManager.AppSettings["RabbitautoAck"].ToString());
        private static IList<SqlBulkCopyColumnMapping> _mappingsColumns = new List<SqlBulkCopyColumnMapping>();

        static async Task Main(string[] args)
        {

            //Crea los directorios
            string _directoryRoot = ConfigurationManager.AppSettings["ProcessJsonPath"].ToString();
            string _DirectoryPath = $"{_directoryRoot}\\pendientes";

            try
            {
                //creo los directorios si no existen
                if (!Directory.Exists(_directoryRoot))
                    Directory.CreateDirectory(_directoryRoot);

                if (!Directory.Exists(_DirectoryPath))
                    Directory.CreateDirectory(_DirectoryPath);
            }
            catch(Exception e) 
            { 
                Console.WriteLine(e.Message);
                }

            //incia el proceso

            ProcessId = Guid.NewGuid();
            string _query = "select * from dbo.RabbitQueue where  estado = 'OnLine' and origen ='Rabbit'";
            //string _query = "select * from RabbitQueue where id= 9";
            DataTable _dt = ADO.SQL.SqlExecuteQueryDataSet(_query, _sqlConnection).Tables[0];
            List<Task> tasks = new List<Task>();
            int _cursoTop = 0;
            foreach (DataRow _r in _dt.Rows)
            {
                string _queueName = _r["QueueName"].ToString();
                string _exchangeName = _r["ExchangeName"].ToString();
                string _routingKey = _r["RoutingKey"].ToString();
                int _RabbieQueueId = (int)_r["Id"];
                

                if (_exchangeName.Length > 1)
                    tasks.Add(GetRabbitQueueEx(_queueName, _exchangeName, _routingKey, _RabbieQueueId,_cursoTop));
                else
                {
                    tasks.Add(GetRabbitQueue(_queueName, _RabbieQueueId,_cursoTop));
                }
                _cursoTop++;
                string _queryUpd = ($"update dbo.RabbitQueue set UltimaEjecucion = getdate() where id = {_RabbieQueueId}");
                ADO.SQL.SqlExecuteNonQuery(_queryUpd, _sqlConnection);
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Obtiene los mensajes de rabbit
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="RabbiQueueId"></param>
        static async Task GetRabbitQueue(string queue, int RabbiQueueId, int cursonTop)
        {

            try
            {
                var factory = new ConnectionFactory
                {
                    UserName = _rabbitUser,
                    Password = _rabbitPass,
                    VirtualHost = "/",
                    Uri = new Uri(_rabbitUrl)
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {

                    string queueName = queue;

                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);



                    int _cMsg = 0;
                    Console.WriteLine($" [*] Esperando mensajes en la cola '{queueName}': Messages: {_cMsg}");
                    var _cleft = Console.CursorLeft + 1;                   
                                                            

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += async (model, ea) =>
                    {
                        ProcessId = Guid.NewGuid();
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        
                        _cMsg++;

                        Console.SetCursorPosition(_cleft,cursonTop);
                        Console.WriteLine($" [*] Esperando mensajes en la cola '{queueName}':Messages: {_cMsg}");

                        //Persiste Msg
                        AddMessageRabbit(message, RabbiQueueId);
                        System.Threading.Thread.Sleep(2000);

                    };

                    //AutoAck=true (Confirma la cola y elimina) AutoAck=False (No confirma la cola y no la elimina)
                    channel.BasicConsume(queue: queueName, autoAck: _autoACK, consumer: consumer);

                    await Task.Delay(-1);

                }
            }
            catch (Exception ex)
            {
                LogError(ex, RabbiQueueId);
            }
        }

        static async Task GetRabbitQueueEx(string queue, string exchangeName, string routingkey, int RabbiQueueId, int cursonTop)
        {

            try
            {
                var factory = new ConnectionFactory
                {
                    UserName = _rabbitUser,
                    Password = _rabbitPass,
                    VirtualHost = "/",
                    Uri = new Uri(_rabbitUrl)
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    string queueName = queue;

                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.QueueBind(queue: queueName,exchange: exchangeName,routingKey: routingkey);


                    int _cMsg = 0;

                    Console.WriteLine($" [*] Esperando mensajes en la cola '{queueName}': Messages: {_cMsg}");

                    var _cleft = Console.CursorLeft + 1;
                    

                    var consumer = new EventingBasicConsumer(channel);
                                      

                    consumer.Received += async (model, ea) =>
                    {
                        ProcessId = Guid.NewGuid();
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        string jsonResponse = message;
                        _cMsg++;

                        Console.SetCursorPosition(_cleft, cursonTop);
                        Console.WriteLine($" [*] Esperando mensajes en la cola '{queueName}': Messages: {_cMsg}");

                       
                        if (queueName.Contains("bi2"))
                        {
                            string _directoryRoot = ConfigurationManager.AppSettings["ProcessJsonPath"].ToString();
                            string _DirectoryPath = $"{_directoryRoot}\\pendientes";
                            string filePath = $"{_DirectoryPath}\\{Guid.NewGuid()}.json";

                            File.WriteAllText(filePath, jsonResponse);
                        }
                        else
                        {
                            //Persiste Msg
                            AddMessageRabbit(message, RabbiQueueId);
                            System.Threading.Thread.Sleep(2000);
                        }
                    };

                    //AutoAck=true (Confirma la cola y elimina) AutoAck=False (No confirma la cola y no la elimina)                   

                    channel.BasicConsume(queue: queueName, autoAck: _autoACK, consumer: consumer);

                    await Task.Delay(-1);

                }
            }
            catch (Exception ex)
            {
                LogError(ex, RabbiQueueId);
            }
        }


        /// <summary>
        /// Persite el mensage de rabbit en la DB
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rabbiQueueId"></param>
        static void AddMessageRabbit(string message, int rabbitQueueId)
        {
            try
            {
                message = message.Replace("'", "");
                string _query = string.Format("insert into [dbo].[RabbitMenssage] (Msg,Estado,RabbitQueue_Id,ImportDate,ProcessId) values ('{0}','New',{1}, getdate(),'{2}')", message, rabbitQueueId, ProcessId.ToString());
                ADO.SQL.SqlExecuteNonQuery(_query, _sqlConnection);

                string _queryUpd = ($"update dbo.RabbitQueue set UltimaEjecucion = getdate() where id = {rabbitQueueId}");
                ADO.SQL.SqlExecuteNonQuery(_queryUpd, _sqlConnection);
            }
            catch (Exception ex)
            {
                LogError(ex, rabbitQueueId);
            }
        }


        /// <summary>
        /// Persiste el log de error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="rabbitQueueId"></param>
        static void LogError(Exception ex, int rabbitQueueId)
        {
            Log.Write.WriteException(ex);
            string _queryErr = string.Format("insert into dbo.rabbitErrorLog (ProcessDate,RabbitQueueId,ErrorMessage,Excepcion,ProcessId ) values(getdate(),{0},'{1}','{2}','{3}')", rabbitQueueId, ex.Message, ex.ToString(), ProcessId);
            ADO.SQL.SqlExecuteNonQuery(_queryErr, _sqlConnection);
        }
    }
}
