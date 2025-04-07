using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.ComponentModel;
using System.Configuration;
using OfficeOpenXml;
using System.Drawing;
using System.IO;

namespace ADO
{
    public class Excel
    {
        public static DataSet getExcelData(string filePath)
        {
            DataSet _ds = new DataSet();

            string _conexion = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties = 'Excel 12.0 Xml;HDR=YES';", filePath);

            OleDbConnection _oconn = new OleDbConnection(_conexion);
            OleDbCommand _oCommand = new OleDbCommand();


            try
            {
                _oconn.Open();
                var schemaDataTable = (DataTable)_oconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);


                _oCommand.Connection = _oconn;
                string _query = string.Format("select * from [{0}]", GetSheetsName(schemaDataTable));
                _oCommand.CommandType = CommandType.Text;
                _oCommand.CommandText = _query;

                (new OleDbDataAdapter(_oCommand)).Fill(_ds);
            }
            catch (OleDbException eole)
            {
                throw eole;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _oCommand.Connection.Close();
                _oCommand.Connection.Dispose();
            }

            return _ds;
        }

        public static DataSet getExcelDataByQuery(string filePath,string query)
        {
            DataSet _ds = new DataSet();

            string _conexion = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties = 'Excel 12.0 Xml;HDR=YES'; ", filePath);

            OleDbConnection _oconn = new OleDbConnection(_conexion);
            OleDbCommand _oCommand = new OleDbCommand();


            try
            {
                _oconn.Open();
                var schemaDataTable = (DataTable)_oconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                _oCommand.Connection = _oconn;
                string _query = query;
                _oCommand.CommandType = CommandType.Text;
                _oCommand.CommandText = _query;

                (new OleDbDataAdapter(_oCommand)).Fill(_ds);
            }
            catch (OleDbException eole)
            {
                throw eole;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _oCommand.Connection.Close();
                _oCommand.Connection.Dispose();
            }

            return _ds;
        }

        public static DataSet getExcelData(string filePath, string sheetName)
        {
            DataSet _ds = new DataSet();

            string _conexion = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties = 'Excel 12.0 Xml;HDR=YES'; ", filePath);

            OleDbConnection _oconn = new OleDbConnection(_conexion);
            OleDbCommand _oCommand = new OleDbCommand();


            try
            {
                _oconn.Open();
                var schemaDataTable = (DataTable)_oconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);


                _oCommand.Connection = _oconn;
                string _query = string.Format("select * from [{0}]", sheetName);
                _oCommand.CommandType = CommandType.Text;
                _oCommand.CommandText = _query;

                (new OleDbDataAdapter(_oCommand)).Fill(_ds);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _oCommand.Connection.Close();
                _oCommand.Connection.Dispose();
            }

            return _ds;
        }

        static string GetSheetsName(DataTable schemaDataTable)
        {
            return schemaDataTable.Rows[0]["TABLE_NAME"].ToString();
        }

        /*
        public static void exportarExcel(string fileName, DataSet dsData)
        {
            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);

            using (ExcelPackage excel = new ExcelPackage())
            {
                foreach (DataTable _dt in dsData.Tables)
                {
                    //Crea las hojas
                    excel.Workbook.Worksheets.Add(_dt.TableName);

                    //Selecciono la hoja a trabajar
                    var worksheet = excel.Workbook.Worksheets[_dt.TableName];

                    //Agrego los nombre de columnas
                    int _colIndex = 1;
                    foreach (DataColumn _column in _dt.Columns)
                    {
                        worksheet.Cells[1, _colIndex].Value = _column.ColumnName;

                        worksheet.Cells[1, _colIndex].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, _colIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, _colIndex].Style.Fill.BackgroundColor.SetColor(Color.Black);

                        _colIndex++;
                    }

                    int _rowIndex = 2;
                    foreach (DataRow _r in _dt.Rows)
                    {

                        for (int _index = 0; _index < _dt.Columns.Count; _index++)
                        {
                            worksheet.Cells[_rowIndex, _index + 1].Value = _r[_index];
                        }

                        _rowIndex++;
                    }

                }
                FileInfo excelFile = new FileInfo(fileName);
                excel.SaveAs(excelFile);
            }
        }
        */

       
    }
}
