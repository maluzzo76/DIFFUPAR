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
    public class DbQueriesController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: DbQueries
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var dbQuery = db.DbQuery.Include(d => d.DbTableStg).Include(d => d.DbTables).Include(m=>m.DbMapping);
            return View(dbQuery.ToList());
        }

        // GET: DbQueries/Details/5
        public ActionResult Details(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbQuery = db.DbQuery.Find(id);

            ViewData["Mapping"] = db.DbMapping.Where(c => c.QueryId == id).Include(i => i.DbColumns).ToList();
            
            if (dbQuery == null)
            {
                return HttpNotFound();
            }
            return View(dbQuery);
        }

        // GET: DbQueries/Create
        public ActionResult Create()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ViewBag.TableDestinoId = new SelectList(db.DbTableStg, "Id", "Name");
            ViewBag.TableId = new SelectList(db.DbTables, "Id", "Name");
            return View();
        }

        // POST: DbQueries/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TableId,TableDestinoId,Where,Name")] DbQuery dbQuery)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.DbQuery.Add(dbQuery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TableDestinoId = new SelectList(db.DbTableStg, "Id", "Name", dbQuery.TableDestinoId);
            ViewBag.TableId = new SelectList(db.DbTables, "Id", "Name", dbQuery.TableId);
            return View(dbQuery);
        }

        // GET: DbQueries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbQuery dbQuery = db.DbQuery.Find(id);
            if (dbQuery == null)
            {
                return HttpNotFound();
            }
            ViewBag.TableDestinoId = new SelectList(db.DbTableStg, "Id", "Name", dbQuery.TableDestinoId);
            ViewBag.TableId = new SelectList(db.DbTables, "Id", "Name", dbQuery.TableId);
            return View(dbQuery);
        }

      

        // POST: DbQueries/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TableId,TableDestinoId,Where,Name")] DbQuery dbQuery)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(dbQuery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TableDestinoId = new SelectList(db.DbTableStg, "Id", "Name", dbQuery.TableDestinoId);
            ViewBag.TableId = new SelectList(db.DbTables, "Id", "Name", dbQuery.TableId);
            return View(dbQuery);
        }

        // GET: DbQueries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbQuery dbQuery = db.DbQuery.Find(id);
            if (dbQuery == null)
            {
                return HttpNotFound();
            }
            return View(dbQuery);
        }

        // POST: DbQueries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            DbQuery dbQuery = db.DbQuery.Find(id);
            db.DbQuery.Remove(dbQuery);
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
