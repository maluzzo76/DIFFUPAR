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
    public class DBProcessesController : Controller
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DBProcesses
        public ActionResult Index()
        {
            return View(db.DBProcess.OrderByDescending(o=> o.Id).ToList());
        }

        public ActionResult updateEstado(int? id, int? cancel = 0)
        {

            if (id != null)
            {
                DBProcess process = db.DBProcess.Find(id);
                process.EstadoDescripcion = "";
                process.Estado = (cancel == 1) ? "Cancelado" : "Pendiente";
                db.Entry(process).State = EntityState.Modified;
                db.SaveChanges();

                /*
                if(cancel==1)
                {
                    string _serviceName ="Importación Complementos SAP DW";

                    ServiceController sc = new ServiceController(_serviceName);

                    try
                    {
                        if (sc != null && sc.Status == ServiceControllerStatus.Running)
                        {
                            sc.Refresh();
                        }
                        else
                        {
                            sc.Start();
                        }
                        sc.WaitForStatus(ServiceControllerStatus.Stopped);
                        sc.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al detener el servicio:");
                        Console.WriteLine(ex.Message);
                    }
                }
                */
            }

            return RedirectToAction("Index");
        }

        public ActionResult FileUpload(string pn)
        {
            ViewBag.ProcessName = pn;
            return View();
        }

        public ActionResult upFile(HttpPostedFileBase file, string processName)
        {

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
