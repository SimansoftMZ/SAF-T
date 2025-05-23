using Simansoft.SAFT.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Simansoft.SAFT.Core.Interfaces
{
    public interface ISaftGenerator<T>
    {
        AuditFile ConverterParaSaft(T ficheiroNaOrigem);
        string GenerateJson(AuditFile auditFile);
        string GenerateXml(AuditFile auditFile);
        byte[] GenerateBytesExcel(AuditFile auditFile);
        byte[] GenerateBytesXml(AuditFile auditFile);
        ValidationResult Validate(T saftData); // Validação integrada
    }
}