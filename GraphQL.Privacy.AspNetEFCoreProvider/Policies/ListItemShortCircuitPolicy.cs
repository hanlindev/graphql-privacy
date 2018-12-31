using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Execution;
using GraphQL.Types;

namespace GraphQL.Privacy.Policies
{
    public class ListItemShortCircuitPolicy<T, TNode> : ClaimsPrincipalAuthorizationPolicy<IEnumerable<TNode>>
        where T : ObjectGraphType<TNode>
        where TNode : class
    {
        public IAuthorizationPolicy<TNode> AlternativeNodePolicy { get; set; }

        public ListItemShortCircuitPolicy()
        {
        }

        public ListItemShortCircuitPolicy(IAuthorizationPolicy<TNode> alternative)
        {
            AlternativeNodePolicy = alternative;
        }

        public override async Task<AuthorizationResult> AuthorizeAsync()
        {
            var result = AuthContext.Subject as IEnumerable<TNode>;
            var arrayNode = Node as ArrayExecutionNode;
            if (result == null || arrayNode == null) 
            {
                return new Skip($"Result either null or not correct type for validation by {GetType().Name}");
            }
            ExecutionStrategy.SetArrayItemNodes(Context, arrayNode);

            var authTasks = arrayNode.Items.Select<ExecutionNode, Task<AuthorizationResult>>(item =>
            {
                var masterPolicy = item.GraphType.GetPolicy<TNode>() ?? AlternativeNodePolicy;
                if (masterPolicy == null) {
                    return Task.FromResult<AuthorizationResult>(
                        new Skip($"Node policy found to authorize objects of type {item.GraphType.GetType().Name}")
                    );
                }
                return masterPolicy.BuildCopy(Context, item).AuthorizeAsync();
            });
            var authResults = await Task.WhenAll(authTasks);

            return new Allow<IEnumerable<TNode>>($"Always allow array field but will remove unauthorized items. By {GetType().Name}")
            {
                PostProcess = (ExecutionContext context, ExecutionNode node) =>
                {
                    IEnumerable<TNode> items = Enumerable.Zip(result, authResults, (TNode first, AuthorizationResult second) =>
                    {
                        if (second is Deny)
                        {
                            return null;
                        }
                        return first;
                    });
                    var newResult = items.Where(item => item != null).ToList();
                    node.Result = newResult;
                    return Task.FromResult(node);
                }
            };
        }

        public override IAuthorizationPolicy<IEnumerable<TNode>> BuildCopy(ExecutionContext context, ExecutionNode node)
        {
            var copy = new ListItemShortCircuitPolicy<T, TNode>(AlternativeNodePolicy);
            copy.BuildContext(context, node);
            return copy;
        }
    }
}