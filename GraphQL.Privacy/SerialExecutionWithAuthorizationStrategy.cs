using System;
using System.Threading.Tasks;
using System.Transactions;
using GraphQL.Execution;

namespace GraphQL.Privacy
{
    public class SerialExecutionWithAuthorizationStrategy : SerialExecutionStrategy
    {
        protected override async Task<ExecutionNode> ExecuteNodeAsync(
            ExecutionContext context, ExecutionNode node)
        {
            try
            {
                using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var resultNode = await base.ExecuteNodeAsync(context, node);
                    scope.Complete();
                    return resultNode;
                }
            }
            catch (AuthorizationPolicyViolationException)
            {
                // TODO handle authorization policy violation
                node.Result = null;
                return node;
            }
            catch (Exception)
            {
                throw;
            }
        }


        protected override void ValidateNodeResult(ExecutionContext context, ExecutionNode node)
        {
            try {
                var result = this.AuthorizeAsync(context, node).ConfigureAwait(false).GetAwaiter().GetResult();
                result.PostProcess(context, node).ConfigureAwait(false).GetAwaiter().GetResult();
            } catch (AuthorizationPolicyViolationException err)
            {
                throw err;
            }
        }
    }
}