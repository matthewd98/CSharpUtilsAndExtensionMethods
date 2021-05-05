using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Program
{
    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string unsecureString)
        {
            var secureString = new SecureString();
            foreach (var character in unsecureString)
                secureString.AppendChar(character);
            return secureString;
        }

        public static string Decrypt(this string stringToDecrypt, X509Certificate2 x509)
        {
            //https://social.technet.microsoft.com/wiki/contents/articles/30291.visual-c-rsa-encryption-using-certificate.aspx

            if (!x509.HasPrivateKey)
                throw new Exception("x509 certicate does not contain a private key for decryption");

            var rsa = (RSA)x509.PrivateKey!;
            var bytesToDecrypt = Convert.FromBase64String(stringToDecrypt);
            var plainBytes = rsa.Decrypt(bytesToDecrypt, RSAEncryptionPadding.Pkcs1);
            return new ASCIIEncoding().GetString(plainBytes);
        }
    }
}