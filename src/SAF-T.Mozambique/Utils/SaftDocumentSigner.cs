﻿using Simansoft.SAFT.Cryptography.Config;
using Simansoft.SAFT.Cryptography.KeyManagement;
using Simansoft.SAFT.Cryptography.Signing;
using System.Security.Cryptography;

namespace Simansoft.SAFT.Mozambique.Utils
{
    /// <summary>
    /// Provides functionality for signing and verifying SAF-T documents using RSA cryptography.
    /// </summary>
    public class SaftDocumentSigner
    {
        private readonly RsaKeyPairService _keyPairService;
        private readonly RsaHashSigner _hashSigner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaftDocumentSigner"/> class.
        /// </summary>
        public SaftDocumentSigner()
        {
            // 1. Instanciar serviço de chaves RSA
            _keyPairService = new RsaKeyPairService(keySize: new MozambiqueCryptoConfig().KeySize);
// Removed commented-out key generation code to reduce clutter and improve maintainability.
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

        public bool Verify(string dadosParaAssinar, string assinaturaBase64)
        {
            if (string.IsNullOrEmpty(dadosParaAssinar))
                throw new ArgumentException("Dados para assinar não podem ser nulos ou vazios", nameof(dadosParaAssinar));
            if (string.IsNullOrEmpty(assinaturaBase64))
                throw new ArgumentException("Assinatura não pode ser nula ou vazia", nameof(assinaturaBase64));
            return _hashSigner.Verify(dadosParaAssinar, assinaturaBase64);
        }

        public RSA GetRsa() => _keyPairService.GetRsa() ?? throw new InvalidOperationException("Chave RSA não inicializada.");
    }
}