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
            var isSuperAdmin = true;
            var model = new DashboardViewModel();
            model.NewAddedUsers = DataLoader.LoadNewAddedUsers(5);            
            model.Newsfeed = DataLoader.LoadNewfeeds(5);
            model.NewGifts = DataLoader.LoadGifts(5);
            if (isSuperAdmin)
            {
                model.NewAddedAdmins = DataLoader.LoadNewAddedAdmins(5);
            }
            return View(model);
        }
    }
}