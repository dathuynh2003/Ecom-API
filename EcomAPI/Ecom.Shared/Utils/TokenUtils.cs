using System.Security.Cryptography;

namespace Ecom.Shared.Utils
{
    public class TokenUtils
    {
        public static string GenerateToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }
    }
}
