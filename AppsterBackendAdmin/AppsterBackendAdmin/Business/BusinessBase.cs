using AppsterBackendAdmin.Infrastructures.Contracts;
using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Implementations;
using AppsterBackendAdmin.Infrastructures.Security;
using AppsterBackendAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Business
{
    public class BusinessBase
    {
        public IDataAccess Context { get; private set; }
        public const string SupperAdmin = "super admin";
        public const int SuperAdminRoleId = 1;

        public BusinessBase()
        {
            Context = AppsterDataAccess.Instance;
        }

        #region helper
        /// <summary>
        /// Compose access token for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="InvalidUserException"></exception>
        internal string ComposeToken(user user)
        {
            var expiration = DateTimeOffset.Now + TimeSpan.FromDays(2);
            var access = GetAccessType(user.role_id);
            var token = Token.CreateAndSign(user.username, user.image, access, expiration);
            return token.Signature;
        }
        /// <summary>
        /// Get user access type
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="InvalidUserException"></exception>
        internal AccessType GetAccessType(int role)
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
    }
}