using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Infrastructures.Security;
using System.Configuration;

namespace AppsterBackendAdmin.Business
{
    public partial class ModifyDataBusiness
    {
        /// <summary>
        /// Validate if input email is a valid email format string. Return true if valid otherwise return false
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private bool ValidateEmailFormat(string emailAddress)
        {
            var regx = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regx.IsMatch(emailAddress);
        }

        /// <summary>
        /// Validate if the plain password matches the password requirement. If match generate the hash password otherwise throw exception.
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        /// <exception cref="AppsRequiredDataIsNullException"></exception>
        /// <exception cref="AppsInvalidDataFormatException"></exception>
        private static string ValidateAndGeneratePasswordHash(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword)) throw new AppsRequiredDataIsNullException();

            int minLength, maxLength;

            if (!int.TryParse(ConfigurationManager.AppSettings["PasswordMinLength"], out minLength) || minLength < 3)
                minLength = 3;

            if (!int.TryParse(ConfigurationManager.AppSettings["PasswordMaxLength"], out maxLength))
                maxLength = 0;

            if (plainPassword.Length < minLength || (maxLength > 0 && plainPassword.Length > maxLength)) throw new AppsInvalidDataFormatException();

            var hashPass = AccountPasswordHelper.Instance.EncryptPassword(plainPassword);

            return hashPass;
        }

        /// <summary>
        /// Check if the DOB is in the accepted range of age. Return true if in range otherwise return false
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        private static bool ValidateDOBAgaintsAcceptedRange(DateTime dob)
        {
            int maxAge;
            int minAge;

            if (!int.TryParse(ConfigurationManager.AppSettings["MaxAgeAcceptance"], out maxAge))
                maxAge = 80;

            if(!int.TryParse(ConfigurationManager.AppSettings["MinAgeAcceptance"], out minAge))
                minAge = 15;

            if (dob.Year < DateTime.Today.Year - maxAge) return false;

            if (DateTime.Today.Year - dob.Year < minAge || (DateTime.Today.Year - dob.Year == minAge && DateTime.Today.Month > dob.Month))
                return false;

            return true;
        }

        /// <summary>
        /// Check if username matches with the requirement. Throw exception in case of unmatch.
        /// </summary>
        /// <param name="username"></param>
        /// <exception cref="AppsRequiredDataIsNullException"></exception>
        /// <exception cref="AppsInvalidDataFormatException"></exception>
        private static void ValidateUserNameAgainstRequirement(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new AppsRequiredDataIsNullException();

            int maxLength, minLength;

            if (!int.TryParse(ConfigurationManager.AppSettings["UserNameMinLength"], out minLength) || minLength < 1)
                minLength = 1;

            if (!int.TryParse(ConfigurationManager.AppSettings["UserNameMaxLength"], out maxLength))
                maxLength = 0;

            if (username.Length < minLength || (maxLength > 0 && username.Length > maxLength))
                throw new AppsInvalidDataFormatException();
        }
    }
}