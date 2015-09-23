using AppsterBackendAdmin.Infrastructures.Contracts;
using AppsterBackendAdmin.Infrastructures.Implementations;
using AppsterBackendAdmin.Infrastructures.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsterBackendAdmin.Controllers
{
    public class BaseController : Controller
    {
        public IDataAccess Context { get; private set; }
        public AccountPasswordHelper PasswordHelper {get; private set;}

        public BaseController()
        {
            Context = AppsterDataAccess.Instance;
            PasswordHelper = AccountPasswordHelper.Instance;
        }

        T GetTempData<T>(string tempKey)
        {
            if (TempData[tempKey] == null) return default(T);
            return (T)TempData[tempKey];
        }

        void SetTempData<T>(string tempKey, T value)
        {
            TempData[tempKey] = value;
        }
    }
}