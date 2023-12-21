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

namespace CMD_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //AgregarObjetivosTableauDiffupar();
            //AgregarObjetivosTableauRouge();

           Process.IA.ScheduleExcecute();
           
           // menu();
        }

        static void menu()
        {
            try
            {
                Console.WriteLine("*".PadRight(88,char.Parse("*")));
                Console.WriteLine(string.Format("{0} A L U S O F T  -  P R O C E S S  U P D A T E {0}","*".PadRight(21, char.Parse("*"))));
                Console.WriteLine("*".PadRight(88, char.Parse("*")));
                Console.ForegroundColor= ConsoleColor.Green;
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
                Console.WriteLine(_colname.Substring(0,_colname.Length-1));                
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
            catch (Exception ex) {
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
            string _ExlsConnection = @"C:\Users\maria\Downloads\Objetivo DIciembre 2023- Diffupar.xlsx";

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
            string _ExlsConnection = @"C:\Users\maria\Downloads\Objetivo DICIEMBRE 2023 - ROUGE.xlsx";

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
