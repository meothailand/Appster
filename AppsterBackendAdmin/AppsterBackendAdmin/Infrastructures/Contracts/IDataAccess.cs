using AppsterBackendAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsterBackendAdmin.Infrastructures.Contracts
{
    public interface IDataAccess
    {
        #region Get Data
        /// <summary>
        /// get role id from database by role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>role id of type int, -1 if no role with the specified role name is found</returns>
        int GetRole(string roleName);
        /// <summary>
        /// get all roles in database
        /// </summary>
        /// <returns></returns>
        IEnumerable<role> GetRoles();
        user GetUser(Func<user, bool> predicate);
        IEnumerable<user> GetUsers(Func<user, bool> predicate, int? take);
        IEnumerable<user> GetNewAddedUser(int? take, bool getAdminUser = false);
        post GetPost(Func<post, bool> predicate);
        IEnumerable<post> GetPosts(Func<post, bool> predicate);
        @event GetEvent(Func<@event, bool> predicate);
        IEnumerable<@event> GetEvents(Func<@event, bool> predicate);
        gift GetGift(Func<gift, bool> predicate);
        IEnumerable<gift> GetGifts(Func<gift, bool> predicate);
        IEnumerable<dynamic> GetSavePushNotifications(Func<save_push_notifications, bool> predicate, int? take);
        #endregion

        #region CRU Data
        /// <summary>
        /// Suspend user(s) where the predicate is matched
        /// </summary>
        /// <param name="predicate"></param>
        void SuspendUser(Func<user, bool> predicate);
        /// <summary>
        /// Suspend user whose id matches the indicated userId
        /// </summary>
        /// <param name="userId"></param>
        void SuspendUser(int userId);
        #endregion

        #region Delete Data
        /// <summary>
        /// Delete user(s) where the predicate is matched
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteUser(Func<user, bool> predicate);
        /// <summary>
        /// Delete user whose id matches the indicated userId
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(int userId);
        #endregion
    }
}
