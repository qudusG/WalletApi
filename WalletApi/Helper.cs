using System.Security.Claims;

namespace WalletApi
{
    public static class Helper
    {
        public static string GetLoggedUserId(ClaimsPrincipal claims)
        {
            var claimsIdentity = claims.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("Not authenticated.");
            }
            return claimsIdentity.FindFirst("id").Value;
        }
    }
}
