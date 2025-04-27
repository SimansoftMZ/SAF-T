using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simansoft.SAFT.Cryptography.KeyManagement
{
    public interface IKeyPairService
    {
        void GenerateKeyPair(int keySize = 1024);
        string ExportPublicKeyPem();
        string ExportPrivateKeyPem();
        void LoadPrivateKeyFromPem(string pem);
        void LoadPublicKeyFromPem(string pem);
    }

}
