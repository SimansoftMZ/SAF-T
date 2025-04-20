using Simansoft.SAFT.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Simansoft.SAFT.Core.Utils
{
    public static class HashUtility : IHashUtility
    {
        public static string GenerateSha256Hash(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = SHA256.HashData(bytes);
            return Convert.ToHexStringLower(hashBytes);
        }
    }
}