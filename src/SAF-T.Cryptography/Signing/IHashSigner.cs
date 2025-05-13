namespace Simansoft.SAFT.Cryptography.Signing
{
    public interface IHashSigner
    {
        string Sign(string message);
        bool Verify(string message, string signatureBase64);
    }
}