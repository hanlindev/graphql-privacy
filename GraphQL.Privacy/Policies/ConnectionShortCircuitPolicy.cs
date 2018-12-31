using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL.Builders;
using GraphQL.Execution;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Types.Relay.DataObjects;
using static GraphQL.Execution.ExecutionHelper;

namespace GraphQL.Privacy.Policies
{
    public class ConnectionShortCircuitPolicy<T, TNode, TSource> : ClaimsPrincipalAuthorizationPolicy<Connection<TNode>>
        where T : ObjectGraphType<TNode>
        where TNode : class
    {
        public ConnectionBuilder<T, TSource> ConnectionBuilder { get; set; }
        // If set, the T's rules will not be used but instead use the alternative rules.
        public IAuthorizationPolicy<Edge<TNode>> EdgeNodePolicy { get; set; }

        public ConnectionShortCircuitPolicy(ConnectionBuilder<T, TSource> builder, IAuthorizationPolicy<Edge<TNode>> alternative) 
        {
            ConnectionBuilder = builder;
            EdgeNodePolicy = alternative;
        }
        public override async Task<AuthorizationResult> AuthorizeAsync()
        {
            var result = AuthContext.Subject as Connection<TNode>;
            if (result == null) 
            {
                return new Skip($"Result either null or not correct type for validation by {GetType().Name}");
            }
            var edgesArrayNode = BuildEdgesArrayExecutionNode(Context, Node);
            if (edgesArrayNode == null) {
                return new Deny($"{GetType().Name} expects a Connection type. None Connection type given");
            }
            var edgeAuthTasks = edgesArrayNode.Items.Select(edge =>
            {
                var policy = EdgeNodePolicy.BuildCopy(Context, edge);
                return policy.AuthorizeAsync();
            });
            var authResults = await Task.WhenAll(edgeAuthTasks);

            return new Allow<Connection<TNode>>($"Always allow connection field but will filter edges. By {GetType().Name}")
            {
                PostProcess = (ExecutionContext context, ExecutionNode node) =>
                {
                    IEnumerable<Edge<TNode>> edges = Enumerable.Zip(result.Edges, authResults, (Edge<TNode> first, AuthorizationResult second) =>
                    {
                        if (second is Deny)
                        {
                            return null;
                        }
                        return first;
                    });
                    result.Edges = edges.Where(edge => edge != null).ToList();
                    node.Result = result;
                    return Task.FromResult(node);
                }
            };
        }

        public ArrayExecutionNode BuildEdgesArrayExecutionNode(ExecutionContext context, ExecutionNode node)
        {
            if (node is ObjectExecutionNode objectNode)
            {
                ExecutionStrategy.SetSubFieldNodes(context, objectNode);
                // Reference:
                // https://github.com/graphql-dotnet/graphql-dotnet/blob/master/src/GraphQL/Execution/ExecutionStrategy.cs#L167
                
                var fields = CollectFields(context, objectNode.GraphType, node.Field.SelectionSet);
                if (fields.ContainsKey("edges")) {
                    var edgesField = fields["edges"];
                    var schema = context.Schema;
                    var edgesFieldDefinition = GetFieldDefinition(
                        context.Document, schema, objectNode.GetObjectGraphType(schema), edgesField);
                    var edgesNode = ExecutionStrategy.BuildExecutionNode(objectNode, edgesFieldDefinition.ResolvedType, edgesField, edgesFieldDefinition);
                    if (objectNode.Result is Connection<TNode> connection) {
                        if (edgesNode is ArrayExecutionNode edgesArrayNode) {
                            edgesArrayNode.Result = connection.Edges;
                            ExecutionStrategy.SetArrayItemNodes(context, edgesArrayNode);
                            return edgesArrayNode;
                        }
                    }
                }
            }
            return null;
        }

        public override IAuthorizationPolicy<Connection<TNode>> BuildCopy(ExecutionContext context, ExecutionNode node)
        {
            var copy = new ConnectionShortCircuitPolicy<T, TNode, TSource>(ConnectionBuilder, EdgeNodePolicy);
            copy.BuildContext(context, node);
            return copy;
        }
    }
}