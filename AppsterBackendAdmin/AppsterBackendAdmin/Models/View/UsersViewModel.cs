using AppsterBackendAdmin.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Models.View
{
    public class UsersViewModel : ViewModelBase
    {
        public List<User> ListUsers { get; set; }
        public PagingHelper Paging { get; set; }
        public UsersViewModel()
        {
            PageTitle = "Users";
            ListUsers = new List<User>();
        }
    }
}