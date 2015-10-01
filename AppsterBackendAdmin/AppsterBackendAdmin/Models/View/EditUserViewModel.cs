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
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
        public int PostCount { get; set; }
        public string CheckinLocation { get; set; }
        public EditUserViewModel(string title = "Edit User")
        {
            PageTitle = title;
        }
    }
}