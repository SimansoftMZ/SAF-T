using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simansoft.SAFT.Cryptography.Signing
{
    public interface IHashSigner
    {
        string Sign(string message);
        bool Verify(string message, string signatureBase64);
    }
}
