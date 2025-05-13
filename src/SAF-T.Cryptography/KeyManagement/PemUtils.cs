using System.Security.Cryptography;

namespace Simansoft.SAFT.Cryptography.KeyManagement
{
    public static class PemUtils
    {
        public static string ExportPublicKeyToPem(RSA rsa) => rsa.ExportSubjectPublicKeyInfoPem();

        public static string ExportPrivateKeyToPem(RSA rsa) => rsa.ExportRSAPrivateKeyPem();

        public static void ImportPublicKey(RSA rsa, string pem) => rsa.ImportFromPem(pem);

        public static void ImportPrivateKey(RSA rsa, string pem) => rsa.ImportFromPem(pem.ToCharArray());
    }
}