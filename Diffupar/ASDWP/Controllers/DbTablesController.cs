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
    public class DbTablesController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DbTables
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var dbTables = db.DbTables.Include(d => d.DbSource);
            return View(dbTables.ToList());
        }

        // GET: DbTables/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbTables dbTables = db.DbTables.Find(id);
            if (dbTables == null)
            {
                return HttpNotFound();
            }
            return View(dbTables);
        }

        // GET: DbTables/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.DbSourceId = new SelectList(db.DbSource, "Id", "Name");
            return View();
        }

        // POST: DbTables/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DbSourceId,Name")] DbTables dbTables)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.DbTables.Add(dbTables);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DbSourceId = new SelectList(db.DbSource, "Id", "Name", dbTables.DbSourceId);
            return View(dbTables);
        }

        // GET: DbTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbTables dbTables = db.DbTables.Find(id);
            if (dbTables == null)
            {
                return HttpNotFound();
            }
            ViewBag.DbSourceId = new SelectList(db.DbSource, "Id", "Name", dbTables.DbSourceId);
            return View(dbTables);
        }

        // POST: DbTables/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DbSourceId,Name")] DbTables dbTables)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(dbTables).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DbSourceId = new SelectList(db.DbSource, "Id", "Name", dbTables.DbSourceId);
            return View(dbTables);
        }

        // GET: DbTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbTables dbTables = db.DbTables.Find(id);
            if (dbTables == null)
            {
                return HttpNotFound();
            }
            return View(dbTables);
        }

        // POST: DbTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            DbTables dbTables = db.DbTables.Find(id);
            db.DbTables.Remove(dbTables);
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
