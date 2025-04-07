using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;
using System.Security.Cryptography;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Configuration;
using System.CodeDom;
using Log;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Process;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Policy;
using static System.Net.WebRequestMethods;
using Microsoft.AspNet.Identity;

namespace CMD_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {

            TestComplementoMayor();

            //Process.Napse _napse = new Process.Napse();
            //NapseCast.JsonNapseTransform();


            //Obtiene el stock mensual
            //_napse.GetNapseRest();

            //byte[] bytes = Encoding.UTF8.GetBytes("AE3PosxfOJIb/wpdwELA4tsoZQtRQs73xNRpNAg8NDi+spRL1c2KiE840IrU5VyDJA=='");


            ///Microsoft.AspNet.Identity.PasswordHasher _Ph = new PasswordHasher();
            //var _pass = _Ph.HashPassword("Mariano");

            //Process.IA.ProcesarComplementos();

            //Console.WriteLine(_pass);
            Console.ReadLine();

            //Process.NapseCast.JsonNapseTransform();

            //AgregarObjetivosTableauDiffupar();
            //AgregarObjetivosTableauRouge();

           

            //Process.IA.ProcesarComplementos();

            // menu();


            /*
           //Chat Kiko
           while (true)
           {
               var msg = Console.ReadLine();
               if (msg != null)
               {
                   using (var client = new HttpClient())
                   {
                       string _url = "http://alusoft.ddns.net:8083/chat/";
                       var json = ($"{{\"message\":\" {msg} \"}}");
                       var content = new StringContent(json, Encoding.UTF8, "application/json");

                       client.DefaultRequestHeaders.Clear();


                       var _response = client.PostAsync(_url, content).Result;
                       var _res = _response.Content.ReadAsStringAsync().Result;
                       dynamic _r = JObject.Parse(_res);
                       var _resu = _r;
                       Console.WriteLine(_r);
                   }
               }
           }
           */
        }

        static void TestComplementoMayor()
        {
            // int _id = Convert.ToInt32(_row["ID"]);
            string _complementosFolder = ConfigurationManager.AppSettings["ComplementosFolder"].ToString();
            //string _fileName = string.Format("{0}{1}", _complementosFolder, _row["Archivo"]);
            string _fileName = @"C:\Users\MarianoRobertoAluzzo\Downloads\Libro Mayor v2.xlsx";
            Log.Write.WriteError("importando complemento " + _fileName);

            //ActualizarEstadoProcess(_id, "Procesando", "");
            
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
                      //  ADO.SQL.SqlExecuteNonQuery(_delete, _sqlConnection);
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
                   // ADO.SQL.SqlBulkCopyNoDelete("[stg].[JDT1Complementos]", _dsExcel.Tables[0], _mapping, _sqlConnection);
                   // ActualizarEstadoProcess(_id, "Procesado OK", "");
                    Log.Write.WriteError("Datos Insertados");
                    _dsExcel.Dispose();
                }
                catch (Exception ee)
                {
                    _dsExcel.Dispose();
                    Log.Write.WriteException(ee);
                   // ActualizarEstadoProcess(_id, "Error", ee.Message);
                    throw ee;
                }
            }
            catch (Exception ex)
            {
                Log.Write.WriteException(ex);
               // ActualizarEstadoProcess(_id, "Error", ex.Message);
            }
            finally { }
        }
        

        static void menu()
        {
            try
            {
                Console.WriteLine("*".PadRight(88, char.Parse("*")));
                Console.WriteLine(string.Format("{0} A L U S O F T  -  P R O C E S S  U P D A T E {0}", "*".PadRight(21, char.Parse("*"))));
                Console.WriteLine("*".PadRight(88, char.Parse("*")));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Selecione un opcion:(s) - Sap Managment | (p) - ETL Process | (Otra tecla) - Salir");

                switch (Console.ReadLine().ToUpper())
                {
                    case "S":
                        SapManager();
                        break;

                    case "P":
                        Process.IA.ScheduleExcecute();
                        break;

                    default:
                        Console.WriteLine("Exit");
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        static void SapManager()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                //string _query = "select SCHEMA_NAME, TABLE_NAME from SYS.M_TABLES where SCHEMA_NAME ='DIFFUPARSA' and TABLE_NAME='@RBI_MARCA' ;";
                Console.Write("Query:");
                string _query = Console.ReadLine();


                string _sapC = ConfigurationManager.ConnectionStrings["SAP"].ConnectionString;


                DataTable _dt = ADO.SAPHana.getByQuery(_query, _sapC).Tables[0];

                string _colname = "";
                Console.WriteLine(string.Empty.PadRight(80, '-'));
                foreach (DataColumn c in _dt.Columns)
                {
                    _colname += string.Format("{0}\t |", c.ColumnName);
                }
                Console.WriteLine(_colname.Substring(0, _colname.Length - 1));
                Console.WriteLine(string.Empty.PadRight(80, '-'));

                foreach (DataRow _r in _dt.Rows)
                {
                    string _value = "";
                    foreach (DataColumn c in _dt.Columns)
                    {
                        _value += string.Format("{0}\t  ", _r[c.ColumnName].ToString());
                    }
                    Console.WriteLine(_value);
                }
                Console.WriteLine("FIN");
                Console.WriteLine("");
                menu();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
            }
            finally
            {
                menu();
            }
        }

        static void AgregarObjetivosTableauDiffupar()
        {
            string _mySqlConnection = "datasource=192.168.1.66 ;port=3306;username=root;password=Diffupar22!;database=rouge;";
            string _ExlsConnection = @"C:\Users\maria\Downloads\Diff\Objetivos DIffupar Enero.xlsx";

            DataSet _dsExls = ADO.Excel.getExcelData(_ExlsConnection, "OBJETIVO TOTAL$");

            Console.WriteLine("Datos de excel");
            Console.WriteLine(_dsExls.Tables[0].Rows.Count);

            int _index = 0;
            IList<string> _qInsert = new List<string>();

            foreach (DataRow _c in _dsExls.Tables[0].Rows)
            {
                if (_c["Local"].ToString() != "")
                {
                    string _valores = string.Format("'{0}','{1}{3}','{1}',{2}", _c["Local"].ToString(), _c["AÑO"].ToString(), _c["TOTAL DIFF REAL"].ToString(), _c["MES"].ToString());
                    string _query = string.Format("insert into rouge.objetivos_diffupar_rouge (Sucursales,ID_YM_target_rouge,Year_target_Diffupar,target_diffupar_mensual) values ({0})", _valores);

                    ADO.MySQL.MySqlExecuteNonQuery(_query, _mySqlConnection);
                }

                _index++;
            }
            Console.WriteLine("=========================================");
            Console.ReadLine();

        }

        static void AgregarObjetivosTableauRouge()
        {
            string _mySqlConnection = "datasource=192.168.1.66 ;port=3306;username=root;password=Diffupar22!;database=rouge;";
            string _ExlsConnection = @"C:\Users\maria\Downloads\Diff\Objetivo ROUGE Enero.xlsx";

            DataSet _dsExls = ADO.Excel.getExcelData(_ExlsConnection, "OBJETIVO TOTAL$");

            Console.WriteLine("Datos de excel");
            Console.WriteLine(_dsExls.Tables[0].Rows.Count);

            int _index = 0;
            IList<string> _qInsert = new List<string>();

            foreach (DataRow _c in _dsExls.Tables[0].Rows)
            {
                if (_index > 0 && _c["LOCAL"].ToString() != "")
                {

                    string _valores = string.Format("'{0}',{2},{1},'{1}-{3}-01','{1}{3}'", _c["LOCAL"].ToString(), _c["AÑO"].ToString(), _c["TOTAL DIFF REAL"].ToString(), _c["MES"].ToString());
                    string _query = string.Format("insert into rouge.objetivos_rouge (Sucursales,target_rouge_mensual,Year,month_year,ID_YM) values ({0})", _valores);

                    ADO.MySQL.MySqlExecuteNonQuery(_query, _mySqlConnection);
                }

                _index++;
            }
            Console.WriteLine("=========================================");
            Console.ReadLine();

        }
    }
}
