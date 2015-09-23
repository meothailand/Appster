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

        static AppsterDataAccess()
        {
            Instance = new AppsterDataAccess();
        }

        
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

        public IEnumerable<role> GetRoles()
        {
            using (var context = new appsterEntities())
            {
                return context.roles.ToArray();
            }
        }

        public Models.user GetUser(Func<Models.user, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                var result = context.users.SingleOrDefault(predicate);
                return result;
            }
        }

        public IEnumerable<Models.user> GetUsers(Func<Models.user, bool> predicate, int? take)
        {
            using (var context = new appsterEntities())
            {
                var users = context.users.Where(predicate);
                if (take.HasValue && take.Value > 0) return users.Take(take.Value).ToArray();
                return users.ToArray();
            }
        }

        public IEnumerable<user> GetNewAddedUser(int? take, bool getAdminUser = false)
        {
            var roleId = GetRole("user");
            take = take.HasValue && take > 0 ? take.Value : 20;
            using (var context = new appsterEntities())
            {
                var newAddedUsers = new List<user>();
                if (getAdminUser)
                {
                    newAddedUsers = (from users in context.users
                                         where users.role_id != roleId
                                         orderby users.id descending
                                         select users).Take(take.Value).ToList();
                }
                else
                {
                    newAddedUsers = (from users in context.users
                                         where users.role_id == roleId
                                         orderby users.id descending
                                     select users).Take(take.Value).ToList();
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