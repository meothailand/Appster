using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Settings;
using AppsterBackendAdmin.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel credential)
        {
            try
            {
                var accessKey = DataLoader.SignInUser(credential.UserName, credential.Password);
                HttpContext.Session[SiteSettings.LoginSessionName] = accessKey;
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                credential.IsError = true;
                if (ex.GetType() == typeof(LoginFailException) || ex.GetType() == typeof(InvalidUserException))
                {
                    credential.ErrorMessage = ex.Message;
                }
                else
                {
                    credential.ErrorMessage = "Cannot sign in to the system. Unknown error. Contact your web admin for support.";
                }
                return View(credential);
            }
        }
    }
}