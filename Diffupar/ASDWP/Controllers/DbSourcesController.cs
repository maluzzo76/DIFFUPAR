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
    public class DbSourcesController : Controller
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DbSources
        public ActionResult Index()
        {
            var dbSource = db.DbSource.Include(d => d.DbProviders);
            return View(dbSource.ToList());
        }

        // GET: DbSources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSource dbSource = db.DbSource.Find(id);
            if (dbSource == null)
            {
                return HttpNotFound();
            }
            return View(dbSource);
        }

        // GET: DbSources/Create
        public ActionResult Create()
        {
            ViewBag.ProviderId = new SelectList(db.DbProviders, "Id", "Name");
            return View();
        }

        // POST: DbSources/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Connetion,ProviderId")] DbSource dbSource)
        {
            if (ModelState.IsValid)
            {
                db.DbSource.Add(dbSource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderId = new SelectList(db.DbProviders, "Id", "Name", dbSource.ProviderId);
            return View(dbSource);
        }

        // GET: DbSources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSource dbSource = db.DbSource.Find(id);
            if (dbSource == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderId = new SelectList(db.DbProviders, "Id", "Name", dbSource.ProviderId);
            return View(dbSource);
        }

        // POST: DbSources/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Connetion,ProviderId")] DbSource dbSource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dbSource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderId = new SelectList(db.DbProviders, "Id", "Name", dbSource.ProviderId);
            return View(dbSource);
        }

        // GET: DbSources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSource dbSource = db.DbSource.Find(id);
            if (dbSource == null)
            {
                return HttpNotFound();
            }
            return View(dbSource);
        }

        // POST: DbSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DbSource dbSource = db.DbSource.Find(id);
            db.DbSource.Remove(dbSource);
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
