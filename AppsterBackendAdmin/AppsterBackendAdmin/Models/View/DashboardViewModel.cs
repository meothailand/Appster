using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Models.View
{
    public class DashboardViewModel : ViewModelBase
    {
        public List<user> NewAddedUsers;
        public List<user> NewAddedAdmins;
        public List<post> NewAddedPosts;
    }
}