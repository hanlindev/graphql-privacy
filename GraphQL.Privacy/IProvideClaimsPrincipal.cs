using System.Security.Claims;

namespace GraphQL.Privacy
{
    public interface IProvideClaimsPrincipal
    {
        ClaimsPrincipal User { get; }
    }
}