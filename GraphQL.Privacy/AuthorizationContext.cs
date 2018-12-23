using GraphQL.Execution;
using GraphQL.Types;

namespace GraphQL.Privacy
{
    public class AuthorizationContext<TModel> : IAuthorizationContext<TModel>
    {
        public TModel Subject { get; set; }
        public IViewer Viewer { get; set; }
        public ExecutionContext ExecutionContext { get; set; }
        public ExecutionNode ExecutionNode { get; set; }

        public T Resolve<T>()
        {
            var schema = ExecutionContext.Schema as Schema;
            return schema.DependencyResolver.Resolve<ITypedResolverService<T>>().Get();
        }
    }
}