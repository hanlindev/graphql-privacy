using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static AspNet.Security.OpenIdConnect.Primitives.OpenIdConnectConstants;

namespace GraphQL.Privacy.Sample
{
    public static class SampleClaims
    {
        public const string UserId = Claims.Subject;
        public const string UserName = Claims.Name;
        public const string Role = Claims.Role;

        public static Claim GetClaimByType(IEnumerable<Claim> claims, string type)
        {
            return claims.Where(claim => claim.Type.Equals(type)).SingleOrDefault();
        }
    }
}
