using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security.Cryptography;

namespace AppsterBackendAdmin.Infrastructures.Security
{
    public class AccountPasswordHelper
    {
        public static AccountPasswordHelper Instance { get; private set; }

        static AccountPasswordHelper()
        {
            Instance = new AccountPasswordHelper();
        }
        public string EncryptPassword(string plainPassword)
        {
            using (var sha1 = SHA1Managed.Create())
            {
                var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(GetPassKeyFromConfig() + plainPassword));
                var strBuilder = new StringBuilder(hashBytes.Length*2);
                foreach (var b in hashBytes)
                {
                    strBuilder.Append(b.ToString("x2"));
                }
                return strBuilder.ToString();
            }
        }

        private string GetPassKeyFromConfig()
        {
            var passKey = ConfigurationManager.AppSettings["Passkey"];
            if (string.IsNullOrEmpty(passKey)) throw new NullReferenceException("Password key not found in config file");
            return passKey;
        }
    }
}