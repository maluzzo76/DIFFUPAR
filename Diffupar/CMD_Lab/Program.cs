﻿using System;
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

namespace CMD_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            menu();
        }

        static void menu()
        {
            try
            {
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
    }
}