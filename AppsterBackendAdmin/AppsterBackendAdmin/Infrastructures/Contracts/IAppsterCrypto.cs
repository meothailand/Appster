using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Contracts
{
    public interface IAppsterCrypto
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);

        T Encrypt<T>(T plainValue);
        T Decrypt<T>(byte[] encryptedValue);
    }
}