using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NapseProcessFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string FilePath =$"{ConfigurationManager.AppSettings["FilePath"].ToString()}\\Pendientes";

            foreach (string file in Directory.GetFiles(FilePath))
            {
                string csvFilePath = file;

                string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString.ToString();

                DataTable dataTable = ReadCsvToDataTable(csvFilePath);

                if (dataTable.Rows.Count > 0)
                {
                    BulkInsertIntoSQL(dataTable, connectionString, "rtl.NapseImport");
                    Console.WriteLine("Importación completada con éxito.");
                    spInyectarNapse(connectionString);
                    Console.WriteLine("Inyección Napse Correcta.");

                    string FilePathDest = $"{ConfigurationManager.AppSettings["FilePath"].ToString()}\\Procesados\\{(new FileInfo(file).Name)}";
                    System.IO.File.Move(file, FilePathDest);
                }
                else
                {
                    Console.WriteLine("El archivo CSV está vacío o tiene problemas.");
                }
            }

        }

        static DataTable ReadCsvToDataTable(string filePath)
        {
            DataTable dt = new DataTable();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    throw new Exception("El archivo CSV está vacío.");
                }

                string[] headers = line.Split(';'); // Obtener encabezados
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Replace(".","").Replace(" ","").Replace("$","").Replace("/","").Replace("(","").Replace(")","").Replace("-","").Trim()); // Crear columnas
                    //Console.WriteLine(header.Replace(".", "").Replace(" ", "").Replace("$", "").Replace("/", "").Replace("(", "").Replace(")", "").Replace("-", "").Trim());
                }

                dt.Columns.Add("ColumnaAdd1"); // Crear columnas
                dt.Columns.Add("ColumnaAdd2");
                

                while (!sr.EndOfStream)
                {
                    string dataLine = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(dataLine)) continue; // Evitar líneas vacías

                    string[] rows = dataLine.Split(';');

                    if (rows.Length > dt.Columns.Count)
                    {
                        Console.WriteLine($"Advertencia: Línea con diferente número de columnas detectada: {dataLine}");
                        continue; // O manejarlo de otra forma
                    }

                    dt.Rows.Add(rows);
                }
            }

            return dt;
        }

        static void BulkInsertIntoSQL(DataTable dataTable, string connectionString, string tableName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        static void spInyectarNapse(string connetionString)
        {
            string query = "exec rtl.InyectarNapse";
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = new SqlConnection(connetionString);
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.CommandTimeout = 60000;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    cmd.Dispose();
                }


            }
        }

    }
}
