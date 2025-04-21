using Simansoft.SAFT.Core.Interfaces;
using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Mozambique.Utils
{
    public record class HashUtility : IHashUtility<DocumentoParaHash>
    {
        public string GerarHashSha256(string chave, string dadosCompostosParaHash)
        {
            throw new NotImplementedException();
        }

        public string GerarHashSha256(DocumentoParaHash obj)
        {
            throw new NotImplementedException();
        }
    }
}