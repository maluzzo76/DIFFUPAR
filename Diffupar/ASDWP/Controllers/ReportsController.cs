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
    public class ReportsController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: Reports
        public ActionResult Index(string report)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");
                       
            var _rep = db.MenuSecurity.Where(w=> w.IsActivo == true && w.AspNetUsers.UserName == User.Identity.Name && w.Nombre == "Reportes").ToList<MenuSecurity>();

            return View(_rep);
        }
    }
}