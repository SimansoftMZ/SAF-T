namespace SAFT.Core.Interfaces
{
    public interface ISaftGenerator<T>
    {
        string GenerateXml(T saftData);
        string GenerateJson(T saftData);
    }
}