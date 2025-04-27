using System.Security.Cryptography;

namespace Simansoft.SAFT.Cryptography.Config
{
    public class MozambiqueCryptoConfig : ICryptoConfig
    {
        public HashAlgorithmName HashAlgorithm => HashAlgorithmName.SHA1;
        public RSASignaturePadding Padding => RSASignaturePadding.Pkcs1;
        public int KeySize => 1024;
    }
}