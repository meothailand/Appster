﻿using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Security;
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
            var tokenStr = filterContext.RequestContext.HttpContext.Request.Headers.GetValues(Token.SecurityHeaderName);
            if (tokenStr == null || tokenStr.Count() == 0) throw new UnauthenticatedException();
            var token = Token.ReadTokenFromString(tokenStr[0]);
            if (token == null) throw new InvalidTokenException();
            token.Validate();
            if (!AllowAll && Array.IndexOf(AccessLevel, token.AccessLevel) < 0) throw new UnauthorizedAccessException();
            base.OnActionExecuting(filterContext);
        }
    }
}