using Simansoft.SAFT.Cryptography.Config;
using Simansoft.SAFT.Cryptography.KeyManagement;
using Simansoft.SAFT.Cryptography.Signing;
using System.Security.Cryptography;

namespace Simansoft.SAFT.Mozambique.Utils
{
    public class SaftDocumentSigner
    {
        private readonly RsaKeyPairService _keyPairService;
        private readonly RsaHashSigner _hashSigner;

        public SaftDocumentSigner()
        {
            // 1. Instanciar serviço de chaves RSA
            _keyPairService = new RsaKeyPairService(keySize: new MozambiqueCryptoConfig().KeySize);
            //_keyPairService.GenerateKeyPair(keySize: new MozambiqueCryptoConfig().KeySize);

            // 2. Criar signer com a configuração de Moçambique
            _hashSigner = new RsaHashSigner(
                _keyPairService.GetRsa()!,
                new MozambiqueCryptoConfig()
            );
        }

        public string SignSaftDocument(string dadosParaAssinar) => _hashSigner.Sign(dadosParaAssinar);

        public string ExportPublicKey() => _keyPairService.ExportPublicKeyPem();
        public string ExportPrivateKey() => _keyPairService.ExportPrivateKeyPem();
        public void LoadPrivateKey(string privateKeyPem) => _keyPairService.LoadPrivateKeyFromPem(privateKeyPem);
        public void LoadPublicKey(string publicKeyPem) => _keyPairService.LoadPublicKeyFromPem(publicKeyPem);
        public RSA GetRsa() => _keyPairService.GetRsa() ?? throw new InvalidOperationException("Chave RSA não inicializada.");
    }
}