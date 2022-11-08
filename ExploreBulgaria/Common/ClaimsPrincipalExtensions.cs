using System.Security.Claims;

namespace ExploreBulgaria.Web.Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);     
    }
}
