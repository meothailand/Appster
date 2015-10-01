using AppsterBackendAdmin.Infrastructures.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppsterBackendAdmin.Models;
using AppsterBackendAdmin.Infrastructures.Security;
using AppsterBackendAdmin.Infrastructures.Exceptions;
using TwinkleStars.Infrastructure.Utils;
using System.Threading.Tasks;

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

        public IEnumerable<gift_categories> GetGiftCategories()
        {
            var context = new appsterEntities();
            try
            {
                var data = context.gift_categories.ToArray();
                return data;
            }
            finally
            {
                context.Dispose();
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

        public IEnumerable<Models.user> GetUsers(Func<Models.user, bool> predicate, int? take, int? cursor, bool loadBack = false)
        {
            using (var context = new appsterEntities())
            {
                cursor = cursor.HasValue ? cursor : 0;
                var users = loadBack ?
                     from data in context.users.Where(predicate) 
                     where data.id < cursor.Value
                     select data : 
                     from data in context.users.Where(predicate)
                       where data.id > cursor.Value
                       select data;

                if (take.HasValue && take.Value > 0) return users.Take(take.Value).ToArray();
                return users.ToArray();
            }
        }

        public IEnumerable<user> GetUsersByPage(Func<user, bool> predicate, bool loadBack, int take, int page, out PagingHelper pagingInfo)
        {
            var context = new appsterEntities();
            try
            {
                var totalItems = context.users.Count(predicate);
                pagingInfo = PagingHelper.GetPageInfo(totalItems, page, take);
                var skip = pagingInfo.CurentPage == 0 ? 0 : (pagingInfo.CurentPage - 1) * take;
                if (loadBack)
                {
                    var data = (from users in context.users.Where(predicate)
                                orderby users.id descending
                                select users).Skip(skip).Take(pagingInfo.Count).ToList();
                    return data;
                }
                else
                {
                    var data = (from users in context.users.Where(predicate)
                                select users).Skip(skip).Take(pagingInfo.Count).ToList();
                    return data;
                }
            }
            finally
            {
                context.Dispose();
            }
        }

        public IEnumerable<user> GetNewAddedUser(int take, bool getAdminUser = false)
        {
            var roleId = GetRole("user");
            using (var context = new appsterEntities())
            {
                if (getAdminUser)
                {
                    var newAddedAdmin = (from users in context.users
                                     where users.role_id != roleId
                                     orderby users.id descending
                                     select users).Take(take).ToArray();
                    return newAddedAdmin;
                }
                else
                {
                    var newAddedUsers = (from users in context.users
                                         where users.role_id == roleId
                                         orderby users.id descending
                                     select users).Take(take).ToList();
                    return newAddedUsers;
                }
                
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

        public IEnumerable<Models.@event> GetEvents(Func<Models.@event, bool> predicate, int take, int cursor = 0, bool loadBack = false)
        {
            throw new NotImplementedException();
        }

        public Models.gift GetGift(Func<Models.gift, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.gift> GetGifts(Func<Models.gift, bool> predicate, int take, int cursor = 0, bool loadBack = false)
        {
            var context = new appsterEntities();
            context.Configuration.AutoDetectChangesEnabled = false;
            try
            {
                var data = loadBack ? (from gifts in context.gifts.Where(predicate).OrderByDescending(i => i.id)
                                       where cursor == 0 || gifts.id < cursor
                                       select gifts).Take(take).ToArray() :
                                      (from gifts in context.gifts.Where(predicate)
                                       where gifts.id > cursor
                                       select gifts).Take(take).ToArray();

                return data;
            }
            finally
            {
                context.Configuration.AutoDetectChangesEnabled = true;
                context.Dispose();
            }
        }


        public IEnumerable<dynamic> GetSavePushNotifications(Func<save_push_notifications, bool> predicate, int take)
        {
                var context = new appsterEntities();
                context.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var data = context.save_push_notifications.Where(predicate).OrderByDescending(n => n.id)
                                   .Join(context.users, p => p.resiver_user_id, u => u.id, (p, u) => new
                                   {
                                       Id = p.id,
                                       UserId = p.user_id,
                                       ReceiverId = p.resiver_user_id,
                                       ReceiverName = u.display_name,
                                       Message = p.message,
                                       CreatedDate = p.created
                                   }).Take(take).ToArray();
                    return data;
                }
                finally
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Dispose();
                }
        }

        public async Task<dynamic> GetUserAdditionalInformation(int userId)
        {
            var context = new appsterEntities();
            try
            {
                var followings = context.follows.Count(i => i.id == userId);
                var followers = context.follows.Count(i => i.follow_user_id == userId);
                var posts = context.posts.Count(i => i.user_id == userId);

                return new
                {
                    FollowerCount = followers,
                    FollowingCount = followings,
                    PostCount = posts
                };
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}