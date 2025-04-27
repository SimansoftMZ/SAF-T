using Simansoft.SAFT.Cryptography.Config;
using System.Security.Cryptography;
using System.Text;

namespace Simansoft.SAFT.Cryptography.Signing
{
    public class RsaHashSigner(RSA rsa, ICryptoConfig config) : IHashSigner
    {
        private readonly RSA _rsa = rsa ?? throw new ArgumentNullException(nameof(rsa));
        private readonly ICryptoConfig _config = config ?? throw new ArgumentNullException(nameof(config));

        public string Sign(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message não pode ser nula ou vazia", nameof(message));

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            byte[] bytesAssinados = _rsa.SignData(bytes, _config.HashAlgorithm, _config.Padding);

            return Convert.ToBase64String(bytesAssinados);
        }

        public bool Verify(string message, string signatureBase64)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message não pode ser nula ou vazia", nameof(message));

            if (string.IsNullOrEmpty(signatureBase64))
                throw new ArgumentException("Signature não pode ser nula ou vazia", nameof(signatureBase64));

            byte[] data = Encoding.UTF8.GetBytes(message);
            byte[] signature = Convert.FromBase64String(signatureBase64);

            return _rsa.VerifyData(data, signature, _config.HashAlgorithm, _config.Padding);
        }
    }
}