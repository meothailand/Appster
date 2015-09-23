using AppsterBackendAdmin.Infrastructures.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppsterBackendAdmin.Models;
using AppsterBackendAdmin.Infrastructures.Security;
using AppsterBackendAdmin.Infrastructures.Exceptions;

namespace AppsterBackendAdmin.Infrastructures.Implementations
{
    public class AppsterDataAccess : IDataAccess
    {
        public static IDataAccess Instance { get; private set; }
        public const string SupperAdmin = "super admin";
        public const int SuperAdminRoleId = 1;

        static AppsterDataAccess()
        {
            Instance = new AppsterDataAccess();
        }

        #region helper
        /// <summary>
        /// Compose access token for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="InvalidUserException"></exception>
        private string ComposeToken(user user)
        {
            var expiration = DateTimeOffset.Now + TimeSpan.FromDays(2);
            var access = GetAccessType(user.role_id);
            var token = Token.CreateAndSign(user.username, access, expiration);
            return token.Signature;
        }
        /// <summary>
        /// Get user access type
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="InvalidUserException"></exception>
        private AccessType GetAccessType(int role)
        {
            using (var context = new appsterEntities())
            {
                var roles = context.roles;
                var userRole = roles.SingleOrDefault(i => i.id == role);
                if (userRole == null) throw new InvalidUserException("User role is invalid");
                switch (userRole.name.ToLower())
                {
                    case SupperAdmin: return AccessType.Supper;
                    case "minimum access": return AccessType.Minimum;
                    case "average access": return AccessType.Average;
                    case "maximum access": return AccessType.Max;
                    default: throw new InvalidUserException("User role is invalid");
                }
            }
        }
        #endregion
        /// <summary>
        /// Get the role_id of an role by role name from database
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>role_id of type int, -1 if no role can be found</returns>
        public int GetRole(string roleName)
        {
            using (var context = new appsterEntities())
            {
                var role = context.roles.SingleOrDefault(i => i.name.ToLower() == roleName.ToLower());
                if (role != null) return role.id;
                return -1;
            }
        }

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>string: user token key</returns>
        /// <exception cref="InvalidUserException"></exception>
        /// <exception cref="LoginFailException"></exception>
        public string SignIn(string userName, string password)
        {
            password = AccountPasswordHelper.Instance.EncryptPassword(password);
            var user = GetUser(i => i.username == userName && i.password == password);
            user = user == null ? GetUser(i => i.email == userName && i.password == password) : user;
            if (user == null)
            {
                throw new LoginFailException("User name or password invalid");
            }
            if (user.status != 1) throw new LoginFailException("Account has been deactivated", 603);
            return ComposeToken(user);
        }

        public Models.user GetUser(Func<Models.user, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                var result = context.users.SingleOrDefault(predicate);
                return result;
            }
        }

        public IEnumerable<Models.user> GetUsers(Func<Models.user, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                var users = context.users.Where(predicate).Where(i => i.role_id != SuperAdminRoleId);
                return users.ToArray();
            }
        }

        public IEnumerable<user> GetNewAddedUser(int? take)
        {
            var roleId = GetRole("user");
            take = take.HasValue && take > 0 ? take.Value : 20;
            using (var context = new appsterEntities())
            {

                var newAddedUsers = (from users in context.users
                              where users.role_id == roleId
                              orderby users.id descending
                              select users).Take(take.Value).ToArray();
                foreach (var u in newAddedUsers)
                {
                    var count = (from giftSent in context.send_gifts
                                 where giftSent.user_id == u.id || giftSent.receiver_user_id == u.id
                                 select giftSent).ToArray();
                    u.gift_sent_count = count.Count(i => i.user_id == u.id);
                    u.gift_received_count = count.Count(i => i.receiver_user_id == u.id);
                }

                return newAddedUsers;
            }
        }

        public Models.post GetPost(Func<Models.post, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                var post = context.posts.SingleOrDefault(predicate);
                return post;
            }
        }

        public IEnumerable<Models.post> GetPosts(Func<Models.post, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                var posts = context.posts.Where(predicate);
                return posts.ToArray();
            }
        }

        public Models.@event GetEvent(Func<Models.@event, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.@event> GetEvents(Func<Models.@event, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Models.gift GetGift(Func<Models.gift, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.gift> GetGifts(Func<Models.gift, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}