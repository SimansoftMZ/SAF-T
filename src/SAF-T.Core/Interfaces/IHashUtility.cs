using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simansoft.SAFT.Core.Interfaces
{
    public interface IHashUtility<T>
    {
        /// <summary>
        /// Gera um hash SHA256 a partir de uma string.
        /// </summary>
        /// <param name="DadosCompostosParaHash">A string a ser convertida em hash</param>
        /// <returns>O hash SHA256 da string.</returns>
        string GerarHashSha256(string chave, string DadosCompostosParaHash);
        /// <summary>
        /// Gera um hash SHA256 a partir de um objeto.
        /// </summary>
        /// <param name="obj">O objeto a ser convertido em hash.</param>
        /// <returns>O hash SHA256 do objeto.</returns>
        string GerarHashSha256(T obj);
    }
}