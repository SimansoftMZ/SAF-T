using SAFT.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace SAFT.Core.Interfaces
{
    public interface ISaftGenerator<T>
    {
        string GenerateXml(T saftData);
        string GenerateJson(T saftData);
        AuditFile ConverterParaSaft(T ficheiroNaOrigem);
        ValidationResult Validate(T saftData); // Validação integrada

    }
}