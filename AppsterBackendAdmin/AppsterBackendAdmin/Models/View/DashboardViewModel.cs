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
    }
}