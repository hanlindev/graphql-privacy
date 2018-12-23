using System.Threading.Tasks;

namespace GraphQL.Privacy.Rules
{
    public class AlwaysAllowRule<T> : IAuthorizationRule<T>
    {
        public Task<AuthorizationResult> AuthorizeAsync(IAuthorizationContext<T> context)
        {
            return Task.FromResult<AuthorizationResult>(new Allow<T>(this));
        }
    }
}