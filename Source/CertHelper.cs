using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Program
{
    public static class CertHelper
    {
        public static X509Certificate2? GetCertificateFromThumbprint(string thumbprint)
        {
            var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            certStore.Open(OpenFlags.ReadOnly);
            var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            certStore.Close();
            return certCollection.Count > 0 ? certCollection[0] : null;
        }
		
	    public static IEnumerable<string> GetCertificateFiles(IEnumerable<string> paths)
        {
            var files = new List<string>();

            foreach (var path in paths)
            {
                if (!Directory.Exists(path)) continue;

                // When retrieving certificates, ignore private key(s) and CA certificate(s).
                files.AddRange(Directory.GetFiles(path, "*.pem", SearchOption.TopDirectoryOnly)
                    .Where(file => !Regex.IsMatch(file, @"(.*key.*\.pem)|(.*cacert.*\.pem)", RegexOptions.IgnoreCase)));
            }

            return files;
        }
	
    }
}