using System;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Privacy.Rules
{
    public class AllowIfViewerIsOwnerRule<T> : IAuthorizationRule<T>
    {
        public Func<T, long?> GetOwnerId { get; private set; }
        public AllowIfViewerIsOwnerRule(Func<T, long?> getOwnerId)
        {
            GetOwnerId = getOwnerId;
        }

        public Task<AuthorizationResult> AuthorizeAsync(IAuthorizationContext<T> context)
        {
            var id = GetOwnerId(context.Subject);
            var viewerId = context.Viewer.User.GetViewerId();
            if (viewerId.Equals(id))
            {
                return Task.FromResult<AuthorizationResult>(new Allow<T>(this));
            }
            return Task.FromResult<AuthorizationResult>(new Skip("Viewer is not owner or subject doesn't have owner ID"));
        }
    }
}
