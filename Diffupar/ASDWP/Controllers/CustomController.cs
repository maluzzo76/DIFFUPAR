using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Security;
using System.Security.Principal;
using ASDWP.Models;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Configuration;

namespace ASDWP.Controllers
{
    public class CustomController : Controller
    {
        private ASDW_Entities db = new ASDW_Entities();

        internal static string _sqlConnexionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public bool validarLoggin()        {
            

            ViewData["Menu"] = db.MenuSecurity.Include(u => u.AspNetUsers).Include(i => i.ItemMenuSecurity).Where(w => w.AspNetUsers.UserName == User.Identity.Name && w.IsActivo == true).OrderBy(o => o.Orden).ToList<MenuSecurity>();
            

            if (User.Identity.Name == "")
                return false;

            return true;
        }

        public AspNetUsers GetUsert()
        {
            var _users = db.AspNetUsers.Where(w => w.Email == User.Identity.Name);
            if (_users.Count() > 0)
                return (AspNetUsers)_users.ToList<AspNetUsers>().ElementAt(0);

            return new AspNetUsers();
        }

        public DataSet SqlExecute(string query)
        {
            DataSet _ds = new DataSet();
            SqlConnection _sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand _sqlCommand = new SqlCommand(query, _sqlConn))
            {
                new SqlDataAdapter(_sqlCommand).Fill(_ds);
                _sqlCommand.Connection.Close();
                _sqlCommand.Connection.Dispose();
            }

            return _ds;
        }

    }
}