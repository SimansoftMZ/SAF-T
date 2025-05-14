using System.Security.Cryptography;

namespace Simansoft.SAFT.Cryptography.Config
{
    public interface ICryptoConfig
    {
        HashAlgorithmName HashAlgorithm { get; }
        RSASignaturePadding Padding { get; }
        int KeySize { get; }
    }
}