using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ASDWP.Models;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;

namespace ASDWP.Controllers
{
    public class ObjetivosController : CustomController
    {
        string _sqlconnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();

        // GET: Objetivos
        public ActionResult Index(int? anio, int? mes, string LugarCliente="")
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.Anio = anio;
            ViewBag.Mes = mes;
            ViewBag.lugarCliente = LugarCliente;

            DataSet _ds = new DataSet();
            string _query = "Select * from stg.objetivos order by anio, mes desc";

            using (SqlConnection _oConn = new SqlConnection(_sqlconnection))
            {
                SqlCommand _Command = new SqlCommand(_query,_oConn);
                (new SqlDataAdapter(_Command)).Fill(_ds);
            }
            DataTable _dt = _ds.Tables[0];
            
            IList<Objetivo> _lobj = new List<Objetivo>();                        

            foreach (DataRow r in _dt.Rows)
            { 
                Objetivo _Nobj = new Objetivo();

                _Nobj.Id = (int)r[0];
                _Nobj.Anio = (int)r[1];
                _Nobj.Mes = (int)r[2];
                //_Nobj.LugarClienteCoidgo = (int)r[3];
                _Nobj.LugarCliente = r[4].ToString().Replace("LOC -","");
                _Nobj.ObjetivoMoneda = (r[5].ToString() == "") ? 0 : decimal.Round((decimal)r[5],2);
                _Nobj.ObjetivoCantidad = (r[6].ToString() == "")?0: decimal.Round((decimal)r[6], 2);
                _Nobj.isNew= bool.Parse(r[7].ToString());
                _Nobj.Origen = r[8].ToString();
                if (r[9].ToString().Length > 6)
                    _Nobj.UltimaModificacion = DateTime.Parse(r[9].ToString());

                _Nobj.Usuario = r[10].ToString();
                _Nobj.PorcentajeDiffupar = (int)r[11];
                

                _lobj.Add(_Nobj);
            }

            Objetivo _fTodos = new Objetivo();

            _fTodos.Anio = DateTime.Now.Year;
            _fTodos.Mes = DateTime.Now.Month;
            _fTodos.LugarClienteCoidgo = 0;
            _fTodos.LugarCliente = "Todos";
            _fTodos.ObjetivoMoneda = 0;
            _fTodos.ObjetivoCantidad = 0;
            _fTodos.isNew = false;
            _fTodos.Origen = "";
            _lobj.Add(_fTodos);

            if (anio == null)
                anio = _lobj.Where(w => w.LugarCliente.Length>0).Max(m => m.Anio);

            if (mes == null)
                mes = _lobj.Where(w => w.LugarCliente.Length>0 && w.Anio == anio).Max(m => m.Mes);



            ViewBag.Anio = new SelectList(_lobj.DistinctBy(d => d.Anio).ToList<Objetivo>(), "Anio", "Anio", anio);
            ViewBag.Mes = new SelectList(_lobj.DistinctBy(d=>d.Mes).ToList<Objetivo>(), "Mes", "Mes", mes);
            

            var _result = _lobj;          



            if (LugarCliente != "" )
                _result = _lobj.Where(w => w.Anio == anio.Value && w.Mes == mes.Value && w.LugarCliente.ToLower().Contains(LugarCliente.ToLower())).ToList<Objetivo>();
            else
                _result = _lobj.Where(w => w.Anio == anio.Value && w.Mes == mes.Value && w.LugarCliente.Length>0 ).ToList<Objetivo>();

            return View(_result.OrderBy(o=>o.LugarCliente).ToList<Objetivo>());
        }

        public ActionResult Details(int? anio, int? mes, string LugarCliente = "")
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");



            DataSet _ds = new DataSet(); ;
            string _query = "Select * from stg.objetivos order by anio, mes desc";

            using (SqlConnection _oConn = new SqlConnection(_sqlconnection))
            {
                SqlCommand _Command = new SqlCommand(_query, _oConn);
                (new SqlDataAdapter(_Command)).Fill(_ds);
            }
            DataTable _dt = _ds.Tables[0];

            IList<Objetivo> _lobj = new List<Objetivo>();

            foreach (DataRow r in _dt.Rows)
            {
                Objetivo _Nobj = new Objetivo();

                _Nobj.Anio = (int)r[0];
                _Nobj.Mes = (int)r[1];
                _Nobj.LugarClienteCoidgo = (int)r[2];
                _Nobj.LugarCliente = r[3].ToString();
                _Nobj.ObjetivoMoneda = decimal.Round((decimal)r[4], 2);
                _Nobj.ObjetivoCantidad = (decimal)r[5];
                _Nobj.isNew = bool.Parse(r[6].ToString());
                _Nobj.Origen = r[7].ToString();

                _lobj.Add(_Nobj);
            }

            Objetivo _fTodos = new Objetivo();

            _fTodos.Anio = DateTime.Now.Year;
            _fTodos.Mes = DateTime.Now.Month;
            _fTodos.LugarClienteCoidgo = 0;
            _fTodos.LugarCliente = "Todos";
            _fTodos.ObjetivoMoneda = 0;
            _fTodos.ObjetivoCantidad = 0;
            _fTodos.isNew = false;
            _fTodos.Origen = "";
            _lobj.Add(_fTodos);

            if (anio == null)
                anio = _lobj.Where(w => w.LugarClienteCoidgo != 0).Max(m => m.Anio);

            if (mes == null)
                mes = _lobj.Where(w => w.LugarClienteCoidgo != 0).Max(m => m.Mes);



            ViewBag.Anio = new SelectList(_lobj.DistinctBy(d => d.Anio).ToList<Objetivo>(), "Anio", "Anio", anio);
            ViewBag.Mes = new SelectList(_lobj.DistinctBy(d => d.Mes).ToList<Objetivo>(), "Mes", "Mes", mes);


            var _result = _lobj;



            if (LugarCliente != "")
                _result = _lobj.Where(w => w.Anio == anio.Value && w.Mes == mes.Value && w.LugarCliente.Contains(LugarCliente) && w.LugarClienteCoidgo != 0).ToList<Objetivo>();
            else
                _result = _lobj.Where(w => w.Anio == anio.Value && w.Mes == mes.Value && w.LugarClienteCoidgo != 0).ToList<Objetivo>();

            return View(_result.OrderBy(o => o.LugarCliente).ToList<Objetivo>());
        }

        public ActionResult Create(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");


            CrearObjetivos();

            return RedirectToAction("Index");
        }

        public ActionResult EditObjetivo(int? id, int? anio, int? mes, string ObjetivoMoneda, string ObjetivoCantidad, int? PorcentajeDiffupar, string lugarCliente="")
        {            

            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            string _ObjetivoMoneda = (ObjetivoMoneda != null) ? ObjetivoMoneda.ToString().Replace(".","").Replace(",", ".") : "ObjetivoMoneda";
            string _ObjetivoCantidad = (ObjetivoCantidad != null) ? ObjetivoCantidad.ToString().Replace(".","").Replace(",",".") : "ObjetivoCantidad";
            string _PorcentajeDiffupar = (PorcentajeDiffupar != null) ? PorcentajeDiffupar.ToString() : "PorcentajeDiffupar";

            string _query = ($"Update stg.Objetivos set ObjetivoMoneda = {_ObjetivoMoneda} ,ObjetivoCantidad = {_ObjetivoCantidad}, PorcentajeDiffupar = {_PorcentajeDiffupar}, UpdateDate = getdate(),Usuario ='{User.Identity.Name}' where id = {id}");


            using (SqlConnection _oConn = new SqlConnection(_sqlconnection))
            {                
                SqlCommand _Command = new SqlCommand(_query, _oConn);
                _Command.Connection.Open();
                _Command.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Objetivos", new { id = id, anio = anio, mes = mes, LugarCliente = lugarCliente });
        }

        internal void CrearObjetivos()
        {
            using (SqlCommand _sqlCommand = new SqlCommand(($"exec AgregarObjtivos '{User.Identity.Name}'"), (new SqlConnection(_sqlconnection))))
            {
                _sqlCommand.CommandTimeout = 6000;
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }
        }

    }

    public class Objetivo
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int LugarClienteCoidgo { get; set; }
        public string LugarCliente { get; set; }
        public decimal ObjetivoMoneda { get; set; }
        public decimal ObjetivoCantidad { get; set; }
        public bool isNew { get;set; }
        public string Origen { get; set; }
        public DateTime UltimaModificacion { get; set; }

        public string Usuario { get; set; }
        public int PorcentajeDiffupar { get; set; }

    }

}