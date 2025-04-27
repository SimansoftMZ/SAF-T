using System.Security.Cryptography;

namespace Simansoft.SAFT.Cryptography.KeyManagement
{
    public static class PemUtils
    {
        public static string ExportPublicKeyToPem(RSA rsa)
        {
            var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            var base64 = Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks);

            return "-----BEGIN PUBLIC KEY-----\n" + base64 + "\n-----END PUBLIC KEY-----";
        }

        public static string ExportPrivateKeyToPem(RSA rsa)
        {
            var privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            var base64 = Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks);

            return "-----BEGIN PRIVATE KEY-----\n" + base64 + "\n-----END PRIVATE KEY-----";
        }

        public static RSA ImportPublicKey(int keySize, string pem)
        {
            var publicKeyBytes = DecodePem(pem, "PUBLIC KEY");
            RSA rsa = RSA.Create(keySize);
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
            return rsa;
        }

        public static RSA ImportPrivateKey(int keySize, string pem)
        {
            var privateKeyBytes = DecodePem(pem, "PRIVATE KEY");
            RSA rsa = RSA.Create(keySize);
            //rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            rsa.ImportFromPem(pem);
            return rsa;
        }

        private static byte[] DecodePem(string pem, string section)
        {
            string header = $"-----BEGIN {section}-----";
            string footer = $"-----END {section}-----";

            int start = pem.IndexOf(header, StringComparison.Ordinal);
            if (start < 0) throw new InvalidOperationException($"Header {header} não encontrado");

            int end = pem.IndexOf(footer, start, StringComparison.Ordinal);
            if (end < 0) throw new InvalidOperationException($"Footer {footer} não encontrado");

            string base64 = pem.Substring(start + header.Length, end - start - header.Length);
            //return Convert.FromBase64String(base64.Replace("\n", "").Replace("\r", ""));
            return Convert.FromBase64String(base64);
        }
    }
}