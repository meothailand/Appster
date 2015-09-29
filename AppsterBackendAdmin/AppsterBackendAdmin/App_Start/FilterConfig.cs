using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Filters;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GeneralExceptionHandler());
        }
    }
}
