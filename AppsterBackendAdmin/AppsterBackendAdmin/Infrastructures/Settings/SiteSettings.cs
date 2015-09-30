using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Settings
{
    public enum AccountStatus
    {
        Suspended,
        Active,
        Unknown
    }
    public class SiteSettings
    {
        public const string LoginSessionName = "AppsterLoginSession";
        public static AccountStatus UserStatus(int status)
        {
            switch (status)
            {
                case 0: return AccountStatus.Suspended;
                case 1: return AccountStatus.Active;
                default: return AccountStatus.Unknown;
            }
        }

        public static string GetProfileImagePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return ConfigurationManager.AppSettings["MediaProfileDefaultLocation"];
            var path = string.Format("{0}/{1}", ConfigurationManager.AppSettings["MediaProfileImageLocation"], fileName);
            return path;
        }

        public static string GetPostMediaFilePath(string fileName)
        {
            var path = string.Format("{0}/{1}", ConfigurationManager.AppSettings["MediaPostUploadLocation"], fileName);
            return path;
        }

        public static string GetGiftFilePath(string fileName)
        {
            var path = string.Format("{0}/{1}", ConfigurationManager.AppSettings["MediaGiftLocation"], fileName);
            return path;
        }

        public static string GetApiPath(string relativeUrl)
        {
            var path = string.Format("{0}/{1}", ConfigurationManager.AppSettings["ApiBaseURL"], relativeUrl);
            return path;
        }
    }
}