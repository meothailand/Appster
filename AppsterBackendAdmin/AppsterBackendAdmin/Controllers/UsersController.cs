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
            var user = DataLoader.LoadUser(i => i.id == id);
            var model = new EditUserViewModel("User Details");
            model.Value = user;
            var addtionalInfo = await DataLoader.Context.GetUserAdditionalInformation(id);
            ModelObjectHelper.CopyObject(addtionalInfo, model);
            return View(model);
        }

        public ViewResult EditUser(int id)
        {
            var user = DataLoader.LoadUser(i => i.id == id);
            if (user != null)
            {
                var model = new EditUserViewModel() { Value = user };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<object> EditUser(User user)
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

        private string ConvertToString(HttpPostedFileBase file)
        {
            using (var ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                var result = Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
                result = string.Format("[{0}]{1}", file.FileName, result);
                return result;
            }
        }
    }
}