using System.ComponentModel.DataAnnotations;

namespace SAFT.Core.Interfaces
{
    public interface ISaftGenerator<T>
    {
        string GenerateXml(T saftData);
        byte[] GenerateXmlBytes(T saftData); // Para grandes volumes
        string GenerateJson(T saftData);
        ValidationResult Validate(T saftData); // Validação integrada
    }
}