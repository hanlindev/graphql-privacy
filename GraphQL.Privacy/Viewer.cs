using System.Security.Claims;

namespace GraphQL.Privacy
{
    public class Viewer : IViewer
    {
        public ClaimsPrincipal User { get; set; }
    }
}