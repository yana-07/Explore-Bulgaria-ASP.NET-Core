using System.Security.Claims;

namespace ExploreBulgaria.Web.Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string FirstName(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.GivenName);

        public static string LastName(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Surname);

        public static string Email(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Email);
    }
}
