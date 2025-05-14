using System.Security.Cryptography;

namespace Simansoft.SAFT.Cryptography.KeyManagement
{
    public class RsaKeyPairService(int keySize = 1024) : IKeyPairService
    {
        private readonly RSA _rsa = RSA.Create(keySize);

        public string ExportPublicKeyPem() => PemUtils.ExportPublicKeyToPem(_rsa);
        public string ExportPrivateKeyPem() => PemUtils.ExportPrivateKeyToPem(_rsa);

        public void LoadPrivateKeyFromPem(string pem) => PemUtils.ImportPrivateKey(_rsa, pem);
        public void LoadPublicKeyFromPem(string pem) => PemUtils.ImportPublicKey(_rsa, pem);

        public RSA GetRsa() => _rsa;
    }
}