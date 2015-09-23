using AppsterBackendAdmin.Infrastructures.Exceptions;
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
        public ActionResult Index(string UserName, string Password)
        {
            try
            {
                var accessKey = Context.SignIn(UserName, Password);
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                var login = new LoginViewModel() { UserName = UserName, Password = Password };
                login.IsError = true;
                if (ex.GetType() == typeof(LoginFailException) || ex.GetType() == typeof(InvalidUserException))
                {
                    login.ErrorMessage = ex.Message;
                }
                else
                {
                    login.ErrorMessage = "Cannot sign in to the system. Unknown error. Contact your web admin for support.";
                }
                return View(login);
            }
        }
    }
}