using GraphQL.Execution;

namespace GraphQL.Privacy
{
    public interface IAuthorizationContext<out TModel>
    {
        // The Model subject to be authorized by a policy.
        TModel Subject { get; }
        IViewer Viewer { get; }
        ExecutionContext ExecutionContext { get; }
        ExecutionNode ExecutionNode { get; }
        T Resolve<T>();
    }
}