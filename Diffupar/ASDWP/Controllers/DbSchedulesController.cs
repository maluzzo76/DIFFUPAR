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
    public class DbSchedulesController : Controller
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DbSchedules
        public ActionResult Index()
        {
            var dbSchedule = db.DbSchedule.Include(d => d.DbQuery);
            return View(dbSchedule.ToList());
        }

        // GET: DbSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSchedule dbSchedule = db.DbSchedule.Find(id);
            if (dbSchedule == null)
            {
                return HttpNotFound();
            }
            return View(dbSchedule);
        }

        // GET: DbSchedules/Create
        public ActionResult Create()
        {
            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Name");
            return View();
        }

        // POST: DbSchedules/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,QueryId,StarDate,LastStarDate,Status")] DbSchedule dbSchedule)
        {
            if (ModelState.IsValid)
            {
                db.DbSchedule.Add(dbSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Where", dbSchedule.QueryId);
            return View(dbSchedule);
        }

        // GET: DbSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSchedule dbSchedule = db.DbSchedule.Find(id);
            if (dbSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Where", dbSchedule.QueryId);
            return View(dbSchedule);
        }

        // POST: DbSchedules/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,QueryId,StarDate,LastStarDate,Status")] DbSchedule dbSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dbSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QueryId = new SelectList(db.DbQuery, "Id", "Where", dbSchedule.QueryId);
            return View(dbSchedule);
        }

        // GET: DbSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSchedule dbSchedule = db.DbSchedule.Find(id);
            if (dbSchedule == null)
            {
                return HttpNotFound();
            }
            return View(dbSchedule);
        }

        // POST: DbSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DbSchedule dbSchedule = db.DbSchedule.Find(id);
            db.DbSchedule.Remove(dbSchedule);
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
