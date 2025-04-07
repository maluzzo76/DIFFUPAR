using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASDWP.Models;
using System.ServiceProcess;
using System.Configuration;

namespace ASDWP.Controllers
{
    public class DBProcessesController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DBProcesses
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            DateTime _fecha = DateTime.Now.AddDays(-15);

            return View(db.DBProcess.Where(w=>w.FechaCarga > _fecha).OrderByDescending(o=> o.Id).ToList());
        }

        public ActionResult updateEstado(int? id, int? cancel = 0)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id != null)
            {
                DBProcess process = db.DBProcess.Find(id);
                process.EstadoDescripcion = "";
                process.Estado = (cancel == 1) ? "Cancelado" : "Pendiente";
                db.Entry(process).State = EntityState.Modified;
                db.SaveChanges();
               
            }

            return RedirectToAction("Index");
        }

        public ActionResult FileUpload(string pn)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.ProcessName = pn;
            return View();
        }

        public ActionResult upFile(HttpPostedFileBase file, string processName)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string _fileName = Guid.NewGuid().ToString();
                    string _fileId = string.Format("{0}.{1}", _fileName,(new FileInfo(file.FileName)).Extension);

                    string _server = Request.Path;
                    string path = Path.Combine(Server.MapPath("~/Complementos"), _fileId);                    
                    file.SaveAs(path);

                    DBProcess process = new DBProcess();
                    process.FechaCarga = DateTime.Now;
                    process.NombreArchivo = file.FileName;
                    process.Estado = "Pendiente";
                    process.TipoProceso = processName;
                    process.Archivo = _fileId;

                    db.DBProcess.Add(process);
                    db.SaveChanges();
                    
                }
            }

            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
