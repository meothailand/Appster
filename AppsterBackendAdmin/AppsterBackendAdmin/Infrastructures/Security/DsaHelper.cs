using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Security
{
    public class DsaHelper
    {
        public static DSA GetPublicKey()
        {
            return GetDSA("public.key");
        }
        public static DSA GetPrivateKey()
        {
            return GetDSA("private.key");
        }
        public static DSA GetDSA(string filename)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", filename);
            string key = string.Empty;
            using (var reader = File.OpenText(filePath))
            {
                key = reader.ReadToEnd();
            }
            var dsa = DSA.Create();
            dsa.FromXmlString(key);
            return dsa;
        }
    }
}