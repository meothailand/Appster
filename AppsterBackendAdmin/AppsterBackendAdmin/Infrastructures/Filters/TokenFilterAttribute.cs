using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Security;
using AppsterBackendAdmin.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin.Infrastructures.Filters
{
    public class TokenFilterAttribute : ActionFilterAttribute
    {
        public bool AllowAll { get; private set; }
        public AccessType[] AccessLevel { get; private set; }
        public TokenFilterAttribute (params AccessType[] accessLevel)
	    {
            AllowAll = false;
            AccessLevel = accessLevel;
	    }
        public TokenFilterAttribute ()
	    {
            AllowAll = true;
            AccessLevel = new AccessType[]{};
	    }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tokenStr = (string)HttpContext.Current.Session[SiteSettings.LoginSessionName];
            if (tokenStr == null) throw new AppsUnauthenticatedException();
            var token = Token.ReadTokenFromString(tokenStr);
            if (token == null) throw new AppsInvalidTokenException();
            token.Validate();
            if (!AllowAll && Array.IndexOf(AccessLevel, token.AccessLevel) < 0) throw new UnauthorizedAccessException();
            base.OnActionExecuting(filterContext);
        }
    }
}