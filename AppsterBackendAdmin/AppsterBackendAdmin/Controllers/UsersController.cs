using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Settings;
using AppsterBackendAdmin.Models.Business;
using AppsterBackendAdmin.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin.Controllers
{
    public class UsersController : BaseController
    {
        // GET: Users
        public ActionResult Index()
        {
            var model = new ViewModelBase();
            model.PageTitle = "Users";
            return View(model);
        }

        public ViewResult EditUser(int id)
        {
            var user = DataLoader.LoadUser(i => i.id == id);
            if (user != null)
            {
                return View(user);
            }
            return View();
        }

        [HttpPut]
        public async Task<object> EditUser(User user, HttpPostedFileBase profileImage)
        {
            try
            {
                await DataModifier.SaveUser(user);
                return Json(new
                {
                    statusCode = (int)HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                var errCode = ex.GetType().GetType() == typeof(HttpException) ? ((HttpException)ex).ErrorCode : (int)HttpStatusCode.InternalServerError;
                return Json(new
                {
                    statusCode = errCode
                });
            }            
        }

        [HttpPost]
        public async Task<object> CreateUser(User user, HttpPostedFileBase profileImage)
        {
            try
            {
                await DataModifier.SaveUser(user, createNew: true);
                return Json(new
                    {
                        status = (int)HttpStatusCode.OK
                    });
            }
            catch(Exception ex)
            {
                var errCode = ex.GetType().GetType() == typeof(HttpException) ? ((HttpException)ex).ErrorCode : (int)HttpStatusCode.InternalServerError;
                return Json(new
                            {
                                status = errCode
                            });
            }
        }

        [HttpPost]
        public JsonResult SuspendUser(int id)
        {
            try
            {
                DataModifier.Context.SuspendUser(id);
                return Json(new
                    {
                        status = (int)HttpStatusCode.OK,
                        userStatusText = AccountStatus.Suspended.ToString(),
                        userStatus = 0
                    });
            }
            catch (Exception ex)
            {
                return Json(new
                    {
                        status = (int)HttpStatusCode.InternalServerError 
                    });
            }
        }
    }
}