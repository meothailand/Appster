using AppsterBackendAdmin.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin.Controllers
{
    public class GiftController : BaseController
    {
        // GET: Gift
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SuspendGift(int id, bool suspend = true)
        {
            try
            {
                var updateStatus = DataModifier.Context.SuspendGift(id, suspend);
                return Json(new
                {
                    status = (int)HttpStatusCode.OK,
                    giftStatusText = SiteSettings.UserStatus(updateStatus).ToString(),
                    giftStatus = updateStatus
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
    }
}