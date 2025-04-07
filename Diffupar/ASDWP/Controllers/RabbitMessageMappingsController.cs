using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASDWP.Models;

namespace ASDWP.Controllers
{
    public class RabbitMessageMappingsController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: RabbitMessageMappings
        public ActionResult Index(int? queueId)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            //Actualiza las columnas de la table stg en el mapeo
            string _qSp = ($"exec InsertRabbitMessageMappingColumns {queueId}");
      

            using (SqlCommand _sqlCommand = new SqlCommand(_qSp, (new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))))
            {
                _sqlCommand.CommandTimeout = 6000;
                _sqlCommand.Connection.Open();
                _sqlCommand.ExecuteNonQuery();
            }

            ViewBag.RabbitqueueId = queueId;
            ViewBag.RabbitqueueName = db.RabbitQueue.Find(queueId).Nombre;

            return View(db.RabbitMessageMapping.Where(w => w.RabbitQueueId == queueId).ToList());
        }

        public ActionResult CreateMapping(int? queueId, int? id, string JsonAttribute)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            RabbitMessageMapping _nMapping = db.RabbitMessageMapping.Find(id);
            _nMapping.JsonAttribute = JsonAttribute;

            if (ModelState.IsValid)
            {
                db.Entry(_nMapping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { queueId = queueId });
            }

            return RedirectToAction("Index", new { queueId = id });
        }

        // GET: RabbitMessageMappings/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RabbitMessageMapping rabbitMessageMapping = db.RabbitMessageMapping.Find(id);
            if (rabbitMessageMapping == null)
            {
                return HttpNotFound();
            }
            return View(rabbitMessageMapping);
        }

        // GET: RabbitMessageMappings/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: RabbitMessageMappings/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ColumnName,JsonAttribute,RabbitQueueId")] RabbitMessageMapping rabbitMessageMapping)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.RabbitMessageMapping.Add(rabbitMessageMapping);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rabbitMessageMapping);
        }

        // GET: RabbitMessageMappings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RabbitMessageMapping rabbitMessageMapping = db.RabbitMessageMapping.Find(id);
            if (rabbitMessageMapping == null)
            {
                return HttpNotFound();
            }
            return View(rabbitMessageMapping);
        }

        // POST: RabbitMessageMappings/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ColumnName,JsonAttribute,RabbitQueueId")] RabbitMessageMapping rabbitMessageMapping)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(rabbitMessageMapping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { queueId = rabbitMessageMapping.RabbitQueueId });
            }
            return View(rabbitMessageMapping);
        }

        // GET: RabbitMessageMappings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RabbitMessageMapping rabbitMessageMapping = db.RabbitMessageMapping.Find(id);
            if (rabbitMessageMapping == null)
            {
                return HttpNotFound();
            }
            return View(rabbitMessageMapping);
        }

        // POST: RabbitMessageMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            RabbitMessageMapping rabbitMessageMapping = db.RabbitMessageMapping.Find(id);
            db.RabbitMessageMapping.Remove(rabbitMessageMapping);
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
