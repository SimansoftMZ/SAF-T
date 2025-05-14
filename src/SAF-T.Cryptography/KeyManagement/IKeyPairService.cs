namespace Simansoft.SAFT.Cryptography.KeyManagement
{
    public interface IKeyPairService
    {
        string ExportPublicKeyPem();
        string ExportPrivateKeyPem();
        void LoadPrivateKeyFromPem(string pem);
        void LoadPublicKeyFromPem(string pem);
    }
}