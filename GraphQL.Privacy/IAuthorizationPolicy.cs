using System.Threading.Tasks;
using GraphQL.Execution;

namespace GraphQL.Privacy
{
    public interface IAuthorizationPolicy<out T>
    {
        IAuthorizationContext<T> AuthContext { get; }
        IAuthorizationPolicy<T> BuildCopy(ExecutionContext context, ExecutionNode node);
        IViewer GetViewer(ExecutionContext context, ExecutionNode node);
        Task<AuthorizationResult> AuthorizeAsync();
    }
}