using Simansoft.SAFT.Core.Interfaces;
using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Mozambique.Generators
{
    public record class HashUtility : IHashUtility<DocumentoParaHash>
    {
        public string GerarHashSha256(string chave, string dados)
        {
            throw new NotImplementedException();
        }

        public string GerarHashSha256(DocumentoParaHash obj)
        {
            throw new NotImplementedException();
        }
    }
}