using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Models.View
{
    public class ViewModelBase
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

    }
}