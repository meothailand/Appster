using AppsterBackendAdmin.Business;
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
            var business = new LoadDataBusiness();
            var model = new DashboardViewModel();
            model.NewAddedUsers = business.LoadNewAddedUsers(10);
            model.NewAddedAdmins = business.LoadNewAddedAdmins(10);
            model.Newsfeed = business.LoadNewfeeds(5);
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