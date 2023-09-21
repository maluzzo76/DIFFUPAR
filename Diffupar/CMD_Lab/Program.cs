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

namespace CMD_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataSet _ds = new DataSet();

            try
            {
                Process.IA.ScheduleExcecute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }



        }
    }
}
