using System.Security.Cryptography;
using System.Text;

namespace SAFT.Core.Utils
{
    public static class HashUtility
    {
        public static string GenerateSha256Hash(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = SHA256.HashData(bytes);
            return Convert.ToHexStringLower(hashBytes);
        }
    }
}