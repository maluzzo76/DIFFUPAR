using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class Write
    {
        static string _path = "c:\\Temp\\";
        public static void WriteException(Exception ex)
        {
            ValidarRepositorio();
            using (StreamWriter writer = new StreamWriter(_path + "Log_ASImport.txt", true))
            {
                Console.ForegroundColor= ConsoleColor.Red;

                writer.WriteLine(String.Format(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ": " + ex.ToString(), DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                Console.WriteLine(String.Format(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ": " + ex.ToString(), DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));

                if (ex.InnerException != null)
                {
                    writer.WriteLine(ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }

                Console.ForegroundColor = ConsoleColor.Red;

                writer.Close();
            }
        }

        public static void WriteError(String error)
        {
            ValidarRepositorio();
            using (StreamWriter writer = new StreamWriter(_path + "Log_ASImport.txt", true))
            {
                writer.WriteLine(String.Format(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ": " + error, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
                Console.WriteLine(String.Format(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ": " + error, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
            }
        }

        public static void WriteSoloFile(String text)
        {
            ValidarRepositorio();
            using (StreamWriter writer = new StreamWriter(_path + "Log_ASImport.txt", true))
            {
                writer.WriteLine(String.Format(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ": " + text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
              
            }
        }

        public static void WriteTextInConsole(String texto)
        {
            
         Console.WriteLine(texto);
            
        }

        static void ValidarRepositorio()
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }
    }
}
