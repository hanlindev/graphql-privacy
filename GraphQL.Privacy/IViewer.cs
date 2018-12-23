using System.Security.Claims;

namespace GraphQL.Privacy
{
    public interface IViewer
    {
        ClaimsPrincipal User { get; }
    }
}