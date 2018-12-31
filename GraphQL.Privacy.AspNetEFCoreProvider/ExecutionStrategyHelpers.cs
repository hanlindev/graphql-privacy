using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GraphQL.Privacy
{
    class ExecutionStrategyHelpers : IExecutionStrategyHelpers
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
