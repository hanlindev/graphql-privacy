using System.Threading.Tasks;

namespace GraphQL.Privacy.Rules
{
    public class AlwaysDenyRule<T> : IAuthorizationRule<T>
    {
        public Task<AuthorizationResult> AuthorizeAsync(IAuthorizationContext<T> context)
        {
            return Task.FromResult<AuthorizationResult>(new Deny("Denied by AlwaysDenyRule"));
        }
    }
}
