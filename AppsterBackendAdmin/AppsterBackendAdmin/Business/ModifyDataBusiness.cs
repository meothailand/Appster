using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Settings;
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
        /// <exception cref="AppsUsernameAlreadyExistException"></exception>
        /// <exception cref="AppsEmailAlreadyExistException"></exception>
        /// <exception cref="AppsDatabaseExecutionException"></exception>
        /// <exception cref="AppsOutOfAcceptedAgeException"></exception>
        /// <exception cref="AppsRequiredDataIsNullException"></exception>
        /// <exception cref="AppsInvalidDataFormatException"></exception>
        /// <exception cref="AppsDataNotFoundException"></exception>
        /// <exception cref="AppsInvalidEmailFormatException"></exception>
        public async Task<int> SaveUser(User user, bool createNew = false, bool isSupperAdmin = false)
        {
            if (!isSupperAdmin && user.role_id != UserRoleId) throw new AppsUnAuthorizedException();
            if (createNew)
            {
                ValidateUserNameAgainstRequirement(user.username);

                if (!string.IsNullOrWhiteSpace(user.email) && !ValidateEmailFormat(user.email))
                    throw new AppsInvalidEmailFormatException();

                user.password = ValidateAndGeneratePasswordHash(user.password);

                if ((user.dob > DateTime.MinValue) && !ValidateDOBAgaintsAcceptedRange(user.dob))
                    throw new AppsOutOfAcceptedAgeException();

                user.gender = string.IsNullOrWhiteSpace(user.gender) ? GenderEnum.Male.ToString() : user.gender;

                var newUser = new user();
                ModelObjectHelper.CopyObject(user, newUser);
                var id = await Context.CreateUser(newUser);
                return id;
            }
            else
            {
                var dbUser = Context.GetUser(i => i.id == user.id);

                if (dbUser == null) throw new AppsDataNotFoundException("This user is no longer exist");

                if (!string.IsNullOrWhiteSpace(user.password))
                    dbUser.password = ValidateAndGeneratePasswordHash(user.password);

                if (user.dob > DateTime.MinValue && !ValidateDOBAgaintsAcceptedRange(user.dob)) throw new AppsOutOfAcceptedAgeException();

                if (!string.IsNullOrWhiteSpace(user.email) && !ValidateEmailFormat(user.email)) throw new AppsInvalidEmailFormatException();

                dbUser.email = user.email;
                dbUser.dob = user.dob;
                dbUser.image = dbUser.image != user.image && !string.IsNullOrWhiteSpace(user.image) ? user.image : dbUser.image;
                dbUser.display_name = user.display_name;
                dbUser.gender = string.IsNullOrWhiteSpace(user.gender) ? GenderEnum.Male.ToString() : user.gender;

                await Context.UpdateUser(dbUser);
                return user.id;                
            }
        }
    }
}