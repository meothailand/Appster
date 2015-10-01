using AppsterBackendAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStars.Infrastructure.Utils;

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
        IEnumerable<gift_categories> GetGiftCategories();
        user GetUser(Func<user, bool> predicate);
        IEnumerable<user> GetUsers(Func<user, bool> predicate, int? take, int? cursor, bool loadBack = false);
        IEnumerable<user> GetUsersByPage(Func<user, bool> predicate, bool loadBack, int take, int page, out PagingHelper pagingInfo);
        IEnumerable<user> GetNewAddedUser(int take, bool getAdminUser = false);
        post GetPost(Func<post, bool> predicate);
        IEnumerable<post> GetPosts(Func<post, bool> predicate);
        @event GetEvent(Func<@event, bool> predicate);
        IEnumerable<@event> GetEvents(Func<@event, bool> predicate, int take, int cursor = 0, bool loadBack = false);
        gift GetGift(Func<gift, bool> predicate);
        IEnumerable<gift> GetGifts(Func<gift, bool> predicate, int take, int cursor = 0, bool loadBack = false);
        IEnumerable<dynamic> GetSavePushNotifications(Func<save_push_notifications, bool> predicate, int take);
        Task<dynamic> GetUserAdditionalInformation(int userId);
        #endregion

        #region CRU Data
        /// <summary>
        /// Suspend user(s) where the predicate is matched
        /// </summary>
        /// <param name="predicate"></param>
        void SuspendUser(Func<user, bool> predicate);
        /// <summary>
        /// Suspend/activate user whose id matches the indicated userId
        /// </summary>
        /// <param name="userId"></param>
        int SuspendUser(int userId, bool suspend = true);
        /// <summary>
        /// Update existing user which new information
        /// </summary>
        /// <param name="updatedUser"></param>
        /// <returns>status as int</returns>
        /// <exception cref="UsernameAlreadyExistException"></exception>
        /// <exception cref="EmailAlreadyExistException"></exception>
        /// <exception cref="DatabaseExecutionException"></exception>
        Task UpdateUser(user updatedUser);
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        /// <exception cref="UsernameAlreadyExistException"></exception>
        /// <exception cref="EmailAlreadyExistException"></exception>
        /// <exception cref="DatabaseExecutionException"></exception>
        Task<int> CreateUser(user newUser);
        int SuspendGift(int giftId, bool suspend = true);
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
