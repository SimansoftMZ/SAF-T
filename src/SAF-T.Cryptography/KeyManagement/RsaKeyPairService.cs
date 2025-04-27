using System.Security.Cryptography;

namespace Simansoft.SAFT.Cryptography.KeyManagement
{
    public class RsaKeyPairService : IKeyPairService
    {
        private RSA? _rsa;
        private int _keySize;

        public void GenerateKeyPair(int keySize = 1024)
        {
            _keySize = keySize;
            _rsa = RSA.Create(_keySize);
        }

        public string ExportPublicKeyPem() => PemUtils.ExportPublicKeyToPem(_rsa!);
        public string ExportPrivateKeyPem() => PemUtils.ExportPrivateKeyToPem(_rsa!);

        public void LoadPrivateKeyFromPem(string pem) => _rsa = PemUtils.ImportPrivateKey(_keySize, pem);
        public void LoadPublicKeyFromPem(string pem) => _rsa = PemUtils.ImportPublicKey(_keySize, pem);

        public RSA? GetRsa() => _rsa;
    }
}