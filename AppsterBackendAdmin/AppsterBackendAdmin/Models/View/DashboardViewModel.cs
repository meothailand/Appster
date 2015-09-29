using AppsterBackendAdmin.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Models.View
{
    public class DashboardViewModel : ViewModelBase
    {
        public List<User> NewAddedUsers;
        public List<User> NewAddedAdmins;
        public List<Newsfeed> Newsfeed;

        public DashboardViewModel()
        {
            NewAddedAdmins = new List<User>();
            NewAddedUsers = new List<User>();
            Newsfeed = new List<Newsfeed>();
            PageTitle = "Dashboard";
        }
    }
}