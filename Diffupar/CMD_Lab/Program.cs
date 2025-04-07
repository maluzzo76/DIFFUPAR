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
            // Servicio 
            //Process.Napse _napse = new Process.Napse();
            //NapseCast.JsonNapseTransform();

            //Proceso desde file
            Process.RabbitJsonProcess.process();

            Console.WriteLine("Fin");
            Console.ReadLine();

        }
    }
}
