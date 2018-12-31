using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GraphQL.Privacy
{
    public class ExecutionStrategyHelpers : IExecutionStrategyHelpers
    {
        public ExecutionNode BuildExecutionNode(ExecutionNode node, IGraphType resolvedType, Field nodeField, FieldType fieldType)
        {
            return ExecutionStrategy.BuildExecutionNode(
                node,
                resolvedType,
                nodeField,
                fieldType);
        }
    }
}
