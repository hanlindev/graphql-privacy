using System.Linq;
using System.Threading.Tasks;
using GraphQL.Execution;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using static GraphQL.Execution.ExecutionHelper;

namespace GraphQL.Privacy.Policies
{
    public class EdgeNodeShortCircuitPolicy<T, TNode> : ClaimsPrincipalAuthorizationPolicy<Edge<TNode>>
        where T : ObjectGraphType<TNode>
    {
        public override async Task<AuthorizationResult> AuthorizeAsync()
        {
            var result = AuthContext.Subject as Edge<TNode>;
            if (result == null) 
            {
                return new Skip($"Result is null or {result?.GetType()?.Name} is not correct type for validation by {GetType().Name}");
            }

            var nodeNode = BuildNodeExecutionNode(Context, Node);
            return await AuthContext
                .Resolve<T>()
                .AuthorizeAsync<TNode>(Context, nodeNode);
        }

        public ExecutionNode BuildNodeExecutionNode(ExecutionContext context, ExecutionNode node)
        {
            if (node is ObjectExecutionNode objectExecutionNode)
            {
                var fields = CollectFields(context, node.GraphType, node.Field.SelectionSet);
                if (fields.ContainsKey("node")) {
                    var nodeField = fields["node"];
                    var schema = context.Schema;
                    var nodeFieldDefinition = GetFieldDefinition(
                        context.Document, schema, objectExecutionNode.GetObjectGraphType(schema), nodeField);
                    var nodeNode = ExecutionStrategy.BuildExecutionNode(
                        objectExecutionNode, nodeFieldDefinition.ResolvedType, nodeField, nodeFieldDefinition);
                    if (node.Result is Edge<TNode> edgeResult)
                    {
                        nodeNode.Result = edgeResult.Node;
                    }
                    return nodeNode;
                }
            }
            return node;
        }

        public override IAuthorizationPolicy<Edge<TNode>> BuildCopy(ExecutionContext context, ExecutionNode node)
        {
            var copy = new EdgeNodeShortCircuitPolicy<T, TNode>();
            copy.BuildContext(context, node);
            return copy;
        }
    }
}