using Simansoft.SAFT.Cryptography.Config;
using System.Security.Cryptography;
using System.Text;

namespace Simansoft.SAFT.Cryptography.Signing
{
    public class RsaHashSigner(RSA rsa, ICryptoConfig config) : IHashSigner
    {
        private readonly RSA _rsa = rsa ?? throw new ArgumentNullException(nameof(rsa));
        private readonly ICryptoConfig _config = config ?? throw new ArgumentNullException(nameof(config));

        public string Sign(string mensagem)
        {
            if (string.IsNullOrEmpty(mensagem))
                throw new ArgumentException("Mensagem não pode ser nula ou vazia", nameof(mensagem));

            byte[] bytes = Encoding.UTF8.GetBytes(mensagem);
            byte[] bytesAssinados = _rsa.SignData(bytes, _config.HashAlgorithm, _config.Padding);

            return Convert.ToBase64String(bytesAssinados);
        }

        public bool Verify(string mensagem, string assinaturaBase64)
        {
            if (string.IsNullOrEmpty(mensagem))
                throw new ArgumentException("Mensagem não pode ser nula ou vazia", nameof(mensagem));

            if (string.IsNullOrEmpty(assinaturaBase64))
                throw new ArgumentException("Assinatura não pode ser nula ou vazia", nameof(assinaturaBase64));

            byte[] data = Encoding.UTF8.GetBytes(mensagem);
            byte[] signature = Convert.FromBase64String(assinaturaBase64);

            return _rsa.VerifyData(data, signature, _config.HashAlgorithm, _config.Padding);
        }
    }
}