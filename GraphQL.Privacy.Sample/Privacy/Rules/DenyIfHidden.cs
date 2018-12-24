using GraphQL.Privacy.Sample.Models.Abstractions;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Privacy.Rules
{
    public class DenyIfHidden<T> : IAuthorizationRule<T>
        where T : ICanBeHidden
    {
        public Task<AuthorizationResult> AuthorizeAsync(IAuthorizationContext<T> context)
        {
            if (context.Subject.IsHidden)
            {
                return Task.FromResult<AuthorizationResult>(new Deny($"This instance of {context.Subject.GetType().Name} is hidden"));
            }
            return Task.FromResult<AuthorizationResult>(new Skip(this));
        }
    }
}
