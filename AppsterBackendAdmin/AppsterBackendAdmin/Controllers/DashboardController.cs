using AppsterBackendAdmin.Infrastructures.Filters;
using AppsterBackendAdmin.Models.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AppsterBackendAdmin.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            var users = Context.GetNewAddedUser(10);
            var model = new DashboardViewModel();
            model.NewAddedUsers = users.ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [TokenFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}