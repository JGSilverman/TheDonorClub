using System;
using System.Security.Claims;


namespace Donator.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsCurrentUser(this ClaimsPrincipal principal, string id)
        {
            var currentUserId = GetUserId(principal);

            return string.Equals(currentUserId, id, StringComparison.OrdinalIgnoreCase);
        }
    }
}
