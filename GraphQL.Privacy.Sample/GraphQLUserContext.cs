using System.Collections.Generic;
using System.Security.Claims;

namespace GraphQL.Privacy.Sample
{
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User
        {
            get
            {
                var result = new ClaimsPrincipal();
                /**
                 * Adjust these claims to change the user attribute
                 */
                var claims = new List<Claim>
                {
                    new Claim(SampleClaims.UserId, "1"),
                    new Claim(SampleClaims.UserName, "SampleUser"),
                    new Claim(SampleClaims.Role, Roles.Enduser)
                };
                return result;
            }
        }
        public IDependencyResolver DependencyResolver { get; set; }
    }
}