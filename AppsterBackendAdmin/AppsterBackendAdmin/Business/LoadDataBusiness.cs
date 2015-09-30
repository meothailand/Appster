using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Security;
using AppsterBackendAdmin.Models;
using AppsterBackendAdmin.Models.Business;
using AppsterBackendAdmin.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Business
{
    public partial class LoadDataBusiness : BusinessBase
    {
        #region initial
        private LoadDataBusiness() { }
        public static LoadDataBusiness Instance { get; private set; }
        static LoadDataBusiness()
        {
            Instance = new LoadDataBusiness();
        }

        #endregion

        public List<User> LoadNewAddedUsers(int take = 10)
        {
            var result = Context.GetNewAddedUser(take).Select(i => new User(i));
            return result.ToList();
        }

        public List<User> LoadNewAddedAdmins(int take = 10)
        {
            var roles = Context.GetRoles();
            var result = Context.GetNewAddedUser(take, getAdminUser: true).Select(i => new User(i, roles));
            return result.ToList();
        }

        public List<Newsfeed> LoadNewfeeds(int take = 10)
        {
            var feeds = Context.GetSavePushNotifications(i => i.id > 0, take).ToList();
            return feeds.Select(i => new Newsfeed(i)).ToList();
        }

        public List<Gifts> LoadGifts(int take = 10)
        {
            var categories = Context.GetGiftCategories().ToList();
            var listGifts = Context.GetGifts(i => i.id > 0, 5, 0, loadBack: true).Select(g => new Gifts(g, categories));
            return listGifts.ToList();
        }

        public User LoadUser(Func<user, bool> predicate)
        {
            var data = Context.GetUser(predicate);

            if (data != null) return new User(data);
            return null;
        }

        public List<User> LoadUsers(Func<user, bool> predicate, int? take, int? cursor, bool loadBack = false)
        {
            var data = Context.GetUsers(predicate, take, cursor, loadBack).ToList();
            var result = data.Select(i => new User(i)).ToList();
            return result;
        }

        public List<User> LoadUsersByPage(Func<user, bool> predicate, int page, out PagingHelper pagingInfo, bool loadBack = false, int take = 20)
        {
            var data = Context.GetUsersByPage(predicate, loadBack, take, page, out pagingInfo);            
            var result = data.Select(i => new User(i)).ToList();
            return result;
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