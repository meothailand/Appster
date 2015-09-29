using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Models;
using AppsterBackendAdmin.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Business
{
    public partial class ModifyDataBusiness : BusinessBase
    {
        #region initial
        public static ModifyDataBusiness Instance { get; private set; }
        static ModifyDataBusiness()
        {
            Instance = new ModifyDataBusiness();
        }

        #endregion
        /// <summary>
        /// Save updated user or new created user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="createNew"></param>
        /// <returns>User id as int</returns>
        /// <exception cref="UsernameAlreadyExistException"></exception>
        /// <exception cref="EmailAlreadyExistException"></exception>
        /// <exception cref="DatabaseExecutionException"></exception>
        public async Task<int> SaveUser(User user, bool createNew = false)
        {
            if (createNew)
            {
                var dbUser = Context.GetUser(i => i.id == user.id);
                if (dbUser == null) throw new DataNotFoundException("This user is no longer exist");
                dbUser.username = user.username;
                dbUser.display_name = user.display_name;
                dbUser.email = user.email;
                dbUser.password = user.password;
                dbUser.role_id = user.role_id;
                await Context.UpdateUser(dbUser);
                return user.id;
            }
            else
            {
                var newUser = new user();
                ModelObjectHelper.CopyObject(user, newUser);
                var id = await Context.CreateUser(newUser);
                return id;
            }
        }
    }
}