using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin.Controllers
{
    public partial class AjaxCallController : BaseController
    {
        public JsonResult SuspendUser(int userId)
        {
            try
            {
                DataModifier.Context.SuspendUser(userId);
                return Json(new { error = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { error = 1 });
            }
            
        }
    }
}