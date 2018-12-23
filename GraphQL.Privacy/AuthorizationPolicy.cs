using System.Threading.Tasks;
using GraphQL.Execution;

namespace GraphQL.Privacy
{
    /**
     * TKey is the type of the Id field of the current viewer (user)
     */
    public abstract class AuthorizationPolicy<T> : IAuthorizationPolicy<T>
        where T : class
    {
        public IAuthorizationContext<T> AuthContext { get; private set; }
        public ExecutionContext Context { get; private set; }
        public ExecutionNode Node { get; private set; }

        public AuthorizationPolicy()
        {
        }

        public abstract Task<AuthorizationResult> AuthorizeAsync();
        public abstract IViewer GetViewer(ExecutionContext context, ExecutionNode node);

        protected void BuildContext(ExecutionContext context, ExecutionNode node)
        {
            Context = context;
            Node = node;
            AuthContext = new AuthorizationContext<T>
            {
                Subject = node.Result as T,
                Viewer = GetViewer(context, node),
                ExecutionContext = context,
                ExecutionNode = node
            };
        }

        public abstract IAuthorizationPolicy<T> BuildCopy(ExecutionContext context, ExecutionNode node);
    }
}