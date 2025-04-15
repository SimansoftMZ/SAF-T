using SAFT.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace SAFT.Core.Interfaces
{
    public interface ISaftGenerator<T>
    {
        string GenerateXml(AuditFile auditFile);
        string GenerateJson(AuditFile auditFile);
        AuditFile ConverterParaSaft(T ficheiroNaOrigem);
        ValidationResult Validate(T saftData); // Validação integrada

    }
}