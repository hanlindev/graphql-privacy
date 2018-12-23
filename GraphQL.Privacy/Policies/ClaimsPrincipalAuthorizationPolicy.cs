using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL.Execution;

namespace GraphQL.Privacy.Policies
{
    public abstract class ClaimsPrincipalAuthorizationPolicy<TModel> : AuthorizationPolicy<TModel>
        where TModel : class
    {
        public override IViewer GetViewer(ExecutionContext context, ExecutionNode node)
        {
            var userContext = context.UserContext as IProvideClaimsPrincipal;
            if (context.UserContext is IProvideClaimsPrincipal userContextWithClaimsPrincipal)
            {
                return new Viewer
                {
                    User = userContextWithClaimsPrincipal.User
                };
            }
            else
            {
                throw new Exception("ExecutionContext.UserContext does not correctly implement IProvideClaimsPrincipal interface");
            }
        }
    }
}