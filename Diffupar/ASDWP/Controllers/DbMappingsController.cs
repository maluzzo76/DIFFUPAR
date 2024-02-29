using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASDWP.Models;

namespace ASDWP.Controllers
{
    public class DbMappingsController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DbMappings
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var dbMapping = db.DbMapping.Include(d => d.DbQuery).Include(d => d.DbColumns);
            return View(dbMapping.ToList());
        }

        // GET: DbMappings/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbMapping dbMapping = db.DbMapping.Find(id);
            if (dbMapping == null)
            {
                return HttpNotFound();
            }
            return View(dbMapping);
        }

        // GET: DbMappings/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Where");
            ViewBag.ColumnSourceId = new SelectList(db.DbColumns, "Id", "Name");
            return View();
        }

        // POST: DbMappings/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,QueryId,ColumnSourceId,ColumnDestino")] DbMapping dbMapping)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");


            if (ModelState.IsValid)
            {
                db.DbMapping.Add(dbMapping);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Name", dbMapping.QueryId);
            ViewBag.ColumnSourceId = new SelectList(db.DbColumns, "Id", "Name", dbMapping.ColumnSourceId);
            return View(dbMapping);
        }

        // GET: DbMappings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbMapping dbMapping = db.DbMapping.Find(id);
            if (dbMapping == null)
            {
                return HttpNotFound();
            }
            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Name", dbMapping.QueryId);
            ViewBag.ColumnSourceId = new SelectList(db.DbColumns.Where(w=>w.DbtableId == dbMapping.DbQuery.TableId), "Id", "Name", dbMapping.ColumnSourceId);
            return View(dbMapping);
            
        }

        // POST: DbMappings/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,QueryId,ColumnSourceId,ColumnDestino")] DbMapping dbMapping)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(dbMapping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "DbQueries", new { id = dbMapping.QueryId });
            }
            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Where", dbMapping.QueryId);
            ViewBag.ColumnSourceId = new SelectList(db.DbColumns, "Id", "Name", dbMapping.ColumnSourceId);
            return  View(dbMapping);
        }

        // GET: DbMappings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbMapping dbMapping = db.DbMapping.Find(id);
            if (dbMapping == null)
            {
                return HttpNotFound();
            }
            return View(dbMapping);
        }

        // POST: DbMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            DbMapping dbMapping = db.DbMapping.Find(id);

            dbMapping.ColumnSourceId = null;

            db.Entry(dbMapping).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", "DbQueries", new { id = dbMapping.QueryId });
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
