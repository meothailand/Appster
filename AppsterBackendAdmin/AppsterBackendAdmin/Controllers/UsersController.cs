using AppsterBackendAdmin.Business;
using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Settings;
using AppsterBackendAdmin.Models.Business;
using AppsterBackendAdmin.Models.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Controllers
{
    public class UsersController : BaseController
    {
        // GET: Users
        public ActionResult Index(int page = 1)
        {
            var model = new UsersViewModel();
            var pagingInfo = new PagingHelper();
            model.ListUsers = DataLoader.LoadUsersByPage(i => i.role_id == 5, page, out pagingInfo, true, 20);
            model.Paging = pagingInfo;
            model.PageTitle = "Users";
            return View(model);
        }

        public async Task<ViewResult> View(int id)
        {
            var model = await GetUserForView(id, isSuperAdmin:false);
            return View(model);
        }

        public async Task<ViewResult> ViewAdmin(int id)
        {
            var model = await GetUserForView(id, isSuperAdmin: true, modelTile: "Admin Details");
            return View(model);
        }

        public ViewResult EditUser(int id)
        {
            var user = DataLoader.LoadUser(i => i.id == id && i.role_id == BusinessBase.UserRoleId);            
            var model = new EditUserViewModel() { Value = user };
            model.ErrorMessage = user == null ? "This user doesn't exist or you don't have enough right to edit this profile." : "";
            return View(model);
        }

        public ViewResult EditAdmin(int id)
        {
            var user = DataLoader.LoadUser(i => i.id == id && i.role_id != BusinessBase.UserRoleId);           
            var model = new EditUserViewModel() { Value = user };
            model.ErrorMessage = user == null ? "This user doesn't exist or you don't have enough right to edit this profile." : "";
            return View(model);
        }

        [HttpPost]
        public async Task<object> EditUser(User user)
        {
            return await EditAccount(user, isSuperAdmin:false);
        }

        [HttpPost]
        public async Task<object> EditAdmin(User user)
        {
            return await EditAccount(user, isSuperAdmin: true);
        }

        public ViewResult CreateUser()
        {
            return View();
        }

        public ViewResult CreateAdmin()
        {
            return View();
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
        public JsonResult SuspendUser(int id, bool suspend = true)
        {
            try
            {
                var updateStatus = DataModifier.Context.SuspendUser(id, suspend);
                return Json(new
                    {
                        status = (int)HttpStatusCode.OK,
                        userStatusText = SiteSettings.UserStatus(updateStatus).ToString(),
                        userStatus = updateStatus
                    });
            }
            catch (Exception ex)
            {
                var code = ex.GetType().GetType() == typeof(HttpException) ? ((HttpException)ex).ErrorCode :
                           (int)HttpStatusCode.InternalServerError; 
                return Json(new
                    {
                        status = code
                    });
            }
        }

        private async Task<EditUserViewModel> GetUserForView(int id, bool isSuperAdmin, string errorMessage = "default", string modelTile = "User Details")
        {
            var user = isSuperAdmin ? DataLoader.LoadUser(i => i.id == id && i.role_id != BusinessBase.UserRoleId) :
                       DataLoader.LoadUser(i => i.id == id && i.role_id == BusinessBase.UserRoleId);
            var model = new EditUserViewModel(modelTile);
            model.Value = user;
            if (user != null)
            {
                var addtionalInfo = await DataLoader.Context.GetUserAdditionalInformation(id);
                ModelObjectHelper.CopyObject(addtionalInfo, model);
            }
            else
            {
                model.ErrorMessage = errorMessage == "default" ? "Sorry this user doesn't exist or you don't have enough authority to view this user's profile." : errorMessage;
            }
            return model;
        }

        private async Task<object> EditAccount(User user, bool isSuperAdmin)
        {
            var httpClient = new HttpClient();
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
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}