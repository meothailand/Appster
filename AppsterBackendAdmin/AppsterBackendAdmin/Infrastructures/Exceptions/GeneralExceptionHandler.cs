using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin.Infrastructures.Exceptions
{
    public class GeneralExceptionHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            if(ex.GetType() == typeof(System.IO.IOException))
            {
                filterContext.Result = new RedirectResult("/Error/Timeout");
            }
        }
    }
}