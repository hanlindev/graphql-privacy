using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Execution;

namespace GraphQL.Privacy.Policies
{
    public class ShortCircuitPolicy<T> : ClaimsPrincipalAuthorizationPolicy<T>
        where T : class
    {
        public IList<IAuthorizationRule<T>> Requirements { get; set; }
        public override async Task<AuthorizationResult> AuthorizeAsync()
        {
            if (AuthContext?.Subject == null) 
            {
                var type = typeof(T);
                return new Skip($"Skipping authorization for Type {type.Name} because context subject is null.");
            }
            var results = await Evaluate();
            foreach (var result in results)
            {
                if (result is Allow<T> || result is Deny)
                {
                    return result;
                }
            }
            return new Skip(AuthContext.Subject);
        }

        protected virtual async Task<IEnumerable<AuthorizationResult>> Evaluate()
        {
            var tasks = Requirements.Select(requirement => requirement.AuthorizeAsync(AuthContext));
            return await Task.WhenAll(tasks);
        }

        public override IAuthorizationPolicy<T> BuildCopy(ExecutionContext context, ExecutionNode node)
        {
            var copy = new ShortCircuitPolicy<T>(){
                Requirements = Requirements
            };
            copy.BuildContext(context, node);
            return copy;
        }
    }
}