using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Security;
using AppsterBackendAdmin.Models.Business;
using AppsterBackendAdmin.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Business
{
    public partial class LoadDataBusiness : BusinessBase
    {
        #region initial
        public static LoadDataBusiness Instance { get; private set; }
        static LoadDataBusiness()
        {
            Instance = new LoadDataBusiness();
        }

        #endregion

        public List<User> LoadNewAddedUsers(int? take)
        {
            var result = Context.GetNewAddedUser(take).Select(i => new User(i));
            return result.ToList();
        }

        public List<User> LoadNewAddedAdmins(int? take)
        {
            var roles = Context.GetRoles();
            var result = Context.GetNewAddedUser(take, getAdminUser: true).Select(i => new User(i, roles));
            return result.ToList();
        }

        public List<Newsfeed> LoadNewfeeds(int? take)
        {
            var feeds = Context.GetSavePushNotifications(i => i.id > 0, take).ToList();
            return feeds.Select(i => new Newsfeed(i)).ToList();
        }



        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>string: user token key</returns>
        /// <exception cref="InvalidUserException"></exception>
        /// <exception cref="LoginFailException"></exception>
        public string SignInUser(string userName, string password)
        {
            password = AccountPasswordHelper.Instance.EncryptPassword(password);
            var user = Context.GetUser(i => i.username == userName && i.password == password);
            user = user == null ? Context.GetUser(i => i.email == userName && i.password == password) : user;
            if (user == null)
            {
                throw new LoginFailException("User name or password invalid");
            }
            if (user.status != 1) throw new LoginFailException("Account has been deactivated", 603);
            return ComposeToken(user);
        }
    }
}