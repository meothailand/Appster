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
        public async Task<object> EditUser(User user, HttpPostedFileBase profileImage)
        {
            try
            {
                var fileContent = Serialize(profileImage);
                HttpClient httpClient = new HttpClient();
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(profileImage.InputStream), "image", profileImage.FileName);
                var response = await httpClient.PostAsync(SiteSettings.GetApiPath("users/remote_update_profile"), content);
                if (!response.IsSuccessStatusCode) throw new HttpException();
                var fileName = await response.Content.ReadAsStringAsync();
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

        private byte[] Serialize(HttpPostedFileBase file)
        {
            var imageFile = new Bitmap(file.InputStream);
            var formatter = new BinaryFormatter();
            var msStream = new MemoryStream();
            
            formatter.Serialize(msStream, imageFile);
            return msStream.ToArray();
        }
    }
}