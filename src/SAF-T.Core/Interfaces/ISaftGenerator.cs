using Simansoft.SAFT.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Simansoft.SAFT.Core.Interfaces
{
    public interface ISaftGenerator<T>
    {
        string GenerateXml(AuditFile auditFile);
        string GenerateJson(AuditFile auditFile);
        AuditFile ConverterParaSaft(T ficheiroNaOrigem);
        ValidationResult Validate(T saftData); // Validação integrada
    }
}