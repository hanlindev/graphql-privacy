using System;
using System.Threading.Tasks;
using GraphQL.Execution;

namespace GraphQL.Privacy
{
    public abstract class AuthorizationResult
    {
        public string Reason { get; protected set; }
        public Func<ExecutionContext, ExecutionNode, Task<ExecutionNode>> PostProcess { get; set; }
        public AuthorizationResult(string reason) {
            Reason = reason;
            PostProcess = PassThroughtPostProcess;
        }

        private Task<ExecutionNode> PassThroughtPostProcess(ExecutionContext context, ExecutionNode node)
        {
            return Task<ExecutionNode>.FromResult(node);
        }
    }
}