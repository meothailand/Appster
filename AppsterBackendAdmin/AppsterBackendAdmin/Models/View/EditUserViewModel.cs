using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppsterBackendAdmin.Models.Business;

namespace AppsterBackendAdmin.Models.View
{
    public class EditUserViewModel : ViewModelBase
    {
        public User Value { get; set; }
        public EditUserViewModel()
        {
            PageTitle = "Edit User";
        }
    }
}