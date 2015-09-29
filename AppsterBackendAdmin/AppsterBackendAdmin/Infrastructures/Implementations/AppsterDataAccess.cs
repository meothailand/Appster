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
    public partial class AppsterDataAccess : IDataAccess
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
            using (var context = new appsterEntities())
            {
                var result = new List<user>();
                if (getAdminUser)
                {
                    var newAddedAdmin = (from users in context.users
                                     where users.role_id != roleId
                                     orderby users.id descending
                                     select users);
                    result = (take.HasValue && take > 0) ? newAddedAdmin.Take(take.Value).ToList() : newAddedAdmin.ToList();
                }
                else
                {
                    var newAddedUsers = (from users in context.users
                                         where users.role_id == roleId
                                         orderby users.id descending
                                     select users).Take(take.Value).ToList();
                    result = (take.HasValue && take > 0) ? newAddedUsers.Take(take.Value).ToList() : newAddedUsers.ToList();
                }
                
                return result;
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


        public IEnumerable<dynamic> GetSavePushNotifications(Func<save_push_notifications, bool> predicate, int? take)
        {
            using (var context = new appsterEntities())
            {
                var feeds = context.save_push_notifications.Where(predicate).OrderByDescending(n => n.id)
                                   .Join(context.users, p => p.resiver_user_id, u => u.id, (p,u) => new{
                                       Id = p.id,
                                       UserId = p.user_id,
                                       ReceiverId = p.resiver_user_id,
                                       ReceiverName = u.display_name,
                                       Message = p.message,
                                       CreatedDate = p.created
                                   });
                if (take.HasValue && take > 0) feeds = feeds.Take(take.Value);
                return feeds.ToList();         
            }
        }


        public void DeleteUser(Func<user, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var users = context.users.Where(predicate);
                try
                {
                    if (users.Count() > 0)
                    {
                        context.users.RemoveRange(users);                        
                    }
                }
                finally
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.SaveChanges();
                }
                
            }
        }

        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }


        public void SuspendUser(Func<user, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var users = context.users.Where(predicate);
                try
                {
                    foreach (var usr in users)
                    {
                        usr.status = 0;
                        context.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                finally
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.SaveChanges();
                }            
            }
        }

        public void SuspendUser(int userId)
        {
            using (var context = new appsterEntities())
            {
                var dbUser = context.users.SingleOrDefault(i => i.id == userId);
                dbUser.status = 0;
                context.SaveChanges();
            }
        }
    }
}