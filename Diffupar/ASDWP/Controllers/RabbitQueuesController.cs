using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASDWP.Models;
using Microsoft.Ajax.Utilities;

namespace ASDWP.Controllers
{
    public class RabbitQueuesController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: RabbitQueues
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            //ViewData["Messages"] = db.RabbitMenssage.ToList();

            return View(db.RabbitQueue.OrderBy(o=>o.Origen).ThenBy(t=>t.Nombre).ToList());
        }

        // GET: RabbitQueues/Details/5
        public ActionResult Details(int? id, string search = "")
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RabbitQueue rabbitQueue = db.RabbitQueue.Find(id);




            if (search != "")
            {
                ViewData["RabbitMessage"] = db.RabbitMenssage.Where(w => w.RabbitQueue_Id == id && w.JsonTransactionID.Contains(search)).GroupBy(d => d.ProcessId).Select(s => s.FirstOrDefault()).OrderByDescending(o => o.ImportDate).ToList();
            }
            else
            {
                ViewData["RabbitMessage"] = db.RabbitMenssage.Where(w => w.RabbitQueue_Id == id).GroupBy(d => d.ProcessId).Select(s => s.FirstOrDefault()).OrderByDescending(o => o.ImportDate).ToList();
            }

            ViewBag.search = search;

            if (rabbitQueue == null)
            {
                return HttpNotFound();
            }
            return View(rabbitQueue);
        }

        public ActionResult MessageDetails(int? queueId, string processId= "")
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            Guid _processId = Guid.Parse(processId);
            var rabbitMessage = db.RabbitMenssage.Where(w=> w.RabbitQueue_Id == queueId && w.ProcessId == _processId ).OrderByDescending(o => o.ImportDate).ToList<RabbitMenssage>();
            ViewBag.queue = db.RabbitQueue.Find(queueId);
           

            string _filter = "";
            foreach (RabbitMenssage _m in rabbitMessage)
            {
                _filter += ($"'{_m.ProcessId}',");
            }
            _filter = _filter.Substring(0, _filter.Length - 1);            

            string _query = ($"select * from stg.NapseTransactions where MessageProcessId in ({_filter})");

           

            DataSet _ds = new DataSet();
          
                using (SqlCommand _sqlCommand = new SqlCommand(_query, (new SqlConnection(_sqlConnexionString))))
                {
                    _sqlCommand.CommandTimeout = 60000;
                    (new SqlDataAdapter(_sqlCommand)).Fill(_ds);
                }


            ViewData["MsgDW"] = _ds.Tables[0];
           
            return View(rabbitMessage);
        }

        // GET: RabbitQueues/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: RabbitQueues/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,ExchangeName,QueueName,RoutingKey,UltimaEjecucion,ProximaEjecucion,Estado,Origen,tipo,TableDestinoDw")] RabbitQueue rabbitQueue)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                rabbitQueue.Estado = "OnLine";

                db.RabbitQueue.Add(rabbitQueue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rabbitQueue);
        }

        // GET: RabbitQueues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RabbitQueue rabbitQueue = db.RabbitQueue.Find(id);
            if (rabbitQueue == null)
            {
                return HttpNotFound();
            }
            return View(rabbitQueue);
        }

        // POST: RabbitQueues/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,ExchangeName,QueueName,RoutingKey,UltimaEjecucion,ProximaEjecucion,Estado,Origen,tipo,TableDestinoDw")] RabbitQueue rabbitQueue)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(rabbitQueue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rabbitQueue);
        }

        // GET: RabbitQueues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RabbitQueue rabbitQueue = db.RabbitQueue.Find(id);
            if (rabbitQueue == null)
            {
                return HttpNotFound();
            }
            return View(rabbitQueue);
        }

        // POST: RabbitQueues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            RabbitQueue rabbitQueue = db.RabbitQueue.Find(id);
            db.RabbitQueue.Remove(rabbitQueue);
            db.SaveChanges();
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
