using System;
using System.Collections.Generic;
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
        public static AccountStatus UserStatus(int status)
        {
            switch (status)
            {
                case 0: return AccountStatus.Suspended;
                case 1: return AccountStatus.Active;
                default: return AccountStatus.Unknown;
            }
        }
    }
}