using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Models.View
{
    public class ViewModelBase
    {
        public string PageTitle { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

        public ViewModelBase()
        {
            PageTitle = string.Empty;
            IsError = false;
            ErrorMessage = string.Empty;
        }
    }
}