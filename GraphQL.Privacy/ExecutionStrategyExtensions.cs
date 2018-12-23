using System.Threading.Tasks;
using GraphQL.Execution;
using GraphQL.Types;

namespace GraphQL.Privacy
{
    public static class ExecutionStrategyExtensions
    {
        // ExecutionStrategy.cs, ExecuteNodeAsync
        // https://github.com/graphql-dotnet/graphql-dotnet/blob/master/src/GraphQL/Execution/ExecutionStrategy.cs#L167

        public static async Task<AuthorizationResult> AuthorizeAsync(
            this ExecutionStrategy instance, ExecutionContext context, ExecutionNode node)
        {
            AuthorizationResult result = await node.GraphType.AuthorizeAsync<object>(context, node);
            if (result is Deny)
            {
                throw new AuthorizationPolicyViolationException(result);
            }

            result = await node.FieldDefinition.AuthorizeAsync<object>(context, node);
            if (result is Deny)
            {
                throw new AuthorizationPolicyViolationException(result);
            }
            return result;
        }
    }
}