using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ASDWP.Models;
using Microsoft.AspNet.Identity;

namespace ASDWP.Controllers
{
    public class UserSettingController : CustomController
    {
        private ASDW_Entities db = new ASDW_Entities();

        // GET: UserSetting
        public ActionResult Index()
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var aspNetUsers = db.AspNetUsers;
            return View(aspNetUsers.ToList());
        }

        public ActionResult Edit(string id)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);

            return View(aspNetUsers);
        }

        public ActionResult setSeguridad(int id, string isActivo)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            ItemMenuSecurity _imenu = db.ItemMenuSecurity.Find(id);
            _imenu.IsActivo = (isActivo=="on")?true:false;

            db.Entry(_imenu).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Edit", new {id=_imenu.MenuSecurity.User_Id });
        }

        public ActionResult setSeguridadMenu(int id,string HashStore, string isActivo)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            MenuSecurity _imenu = db.MenuSecurity.Find(id);
            _imenu.IsActivo = (isActivo == "on") ? true : false;
            _imenu.OcrCode3 = HashStore;

            db.Entry(_imenu).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Edit", new {id=_imenu.User_Id });
        }

        public ActionResult ResetPassword(string id = "")
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _user = db.AspNetUsers.Find(id);
            
            return View(_user);
        }

        //
        // POST: /Account/ResetPassword        
        public ActionResult ResetPasswordSave(string id, string password, string passwordConfirm)
        {
            if (!validarLoggin())
                return RedirectToAction("Index", "Home");

            var _user = db.AspNetUsers.Find(id);

            if (password.Equals(passwordConfirm))
            {
                Microsoft.AspNet.Identity.PasswordHasher _Ph = new PasswordHasher();
                _user.PasswordHash = _Ph.HashPassword(password);

                db.Entry(_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("ResetPassword","UserSetting",new { id  = _user.Id});
            }

            return RedirectToAction("ResetPassword", "UserSetting");
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
