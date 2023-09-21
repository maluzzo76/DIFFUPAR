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
    public class DbColumnsController : Controller
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DbColumns
        public ActionResult Index()
        {
            var dbColumns = db.DbColumns.Include(d => d.DbTables);
            return View(dbColumns.ToList());
        }

        // GET: DbColumns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbColumns dbColumns = db.DbColumns.Find(id);
            if (dbColumns == null)
            {
                return HttpNotFound();
            }
            return View(dbColumns);
        }

        // GET: DbColumns/Create
        public ActionResult Create()
        {
            ViewBag.DbtableId = new SelectList(db.DbTables, "Id", "Name");
            return View();
        }

        // POST: DbColumns/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DbtableId,Name")] DbColumns dbColumns)
        {
            if (ModelState.IsValid)
            {
                db.DbColumns.Add(dbColumns);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DbtableId = new SelectList(db.DbTables, "Id", "Name", dbColumns.DbtableId);
            return View(dbColumns);
        }

        // GET: DbColumns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbColumns dbColumns = db.DbColumns.Find(id);
            if (dbColumns == null)
            {
                return HttpNotFound();
            }
            ViewBag.DbtableId = new SelectList(db.DbTables, "Id", "Name", dbColumns.DbtableId);
            return View(dbColumns);
        }

        // POST: DbColumns/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DbtableId,Name")] DbColumns dbColumns)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dbColumns).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DbtableId = new SelectList(db.DbTables, "Id", "Name", dbColumns.DbtableId);
            return View(dbColumns);
        }

        // GET: DbColumns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbColumns dbColumns = db.DbColumns.Find(id);
            if (dbColumns == null)
            {
                return HttpNotFound();
            }
            return View(dbColumns);
        }

        // POST: DbColumns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DbColumns dbColumns = db.DbColumns.Find(id);
            db.DbColumns.Remove(dbColumns);
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
