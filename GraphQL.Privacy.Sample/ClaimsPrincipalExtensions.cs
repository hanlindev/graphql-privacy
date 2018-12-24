using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample
{
    public static class ClaimsPrincipalExtensions
    {
        private static Claim GetClaimByType(IEnumerable<Claim> claims, string claimType)
        {
            return claims.Where(claim => claim.Type.Equals(claimType)).SingleOrDefault();
        }
        public static long? GetViewerId(this ClaimsPrincipal user)
        {
            var claimValue = GetClaimByType(user.Claims, SampleClaims.UserId);
            try
            {
                return long.Parse(claimValue.Value);
            }
            catch
            {
                return -1;
            }
        }
    }
}
