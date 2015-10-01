using AppsterBackendAdmin.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Security
{
    [Serializable]
    public enum AccessType
    {
        Supper,
        Max,
        Average,
        Minimum
    }

    [Serializable]
    public sealed class Token
    {
        public const string SecurityHeaderName = "appster-accesskey";
        public string UserName { get; private set; }
        public DateTimeOffset ExpireTime { get; set; }
        public AccessType AccessLevel { get; set; }
        public string ProfilePath { get; set; }
        public string Signature { get; set; }
        private Token(string username, string profileImg, AccessType accessType, DateTimeOffset expiration)
        {
            UserName = username;
            ProfilePath = profileImg;
            AccessLevel = accessType;
            ExpireTime = expiration;
        }
        public static Token CreateAndSign(string username,string profileImg, AccessType accessLevel, DateTimeOffset? expiredTime)
        {
            var expiration = expiredTime.HasValue ? expiredTime.Value : DateTimeOffset.Now + TimeSpan.FromDays(2);
            var token = new Token(username, profileImg, accessLevel, expiration);
            SignToken(token);
            return token;
        }
        public static Token ReadTokenFromHeader(HttpRequestMessage request)
        {
            if (request.Headers.Any(h => h.Key.ToLower() == SecurityHeaderName))
            {
                var header = request.Headers.First(h => h.Key.ToLower() == SecurityHeaderName);
                var signature = header.Value.FirstOrDefault() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(signature)) return null;
                return ReadTokenFromString(signature);
            }
            return null;
        }
        public static Token ReadTokenFromString(string signature)
        {
            signature = signature.Trim();
            var bytes = ConvertToBytes(signature);
            var token = DeserializeToken(bytes);
            token.Signature = signature;
            return token;
        }
        public void Validate()
        {
            if (this.ExpireTime < DateTimeOffset.Now) throw new AppsTokenExpiredException();
            using (var verifier = DsaHelper.GetPublicKey())
            {
                var signature = ConvertToBytes(this.Signature);
                var data = this.ComposeData();
                if (!verifier.VerifySignature(data, signature)) throw new AppsInvalidTokenException();
            }
        }
        private static Token SignToken(Token token)
        {
            using (var signer = DsaHelper.GetPrivateKey())
            {
                var rbgHash = token.ComposeData();
                var sigBytes = signer.CreateSignature(rbgHash);
                var sigStr = ConvertToString(sigBytes);
                token.Signature = sigStr;
            }
            return token;
        }

        private byte[] ComposeData()
        {
            using(var sha1 = SHA1.Create())
            {
                string data = string.Format("{0}{1}{2}{3}", this.UserName, this.AccessLevel, this.ExpireTime, this.ProfilePath);
                var rbgHash = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));
                return rbgHash;
            }
        }
        private static string ConvertToString(byte[] bytes)
        {
            var result = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return result;
        }
        private static byte[] ConvertToBytes(string data)
        {
            var refinedData = data.Trim().Replace('-', '+').Replace('_', '/');
            switch (refinedData.Length % 4)
            {
                case 2: refinedData += "=="; break;
                case 3: refinedData += "="; break;
                default: break;
            }
            var bytes = Convert.FromBase64String(refinedData);
            return bytes;
        }
        private static Token DeserializeToken(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                try
                {                   
                    var token = (Token)formatter.Deserialize(ms);
                    return token;
                }
                catch
                {
                    throw new AppsInvalidTokenException();
                }
            }
        }
    }
}