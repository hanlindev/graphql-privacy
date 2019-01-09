using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Privacy
{
    public interface IExecutionStrategyHelpers
    {
        ExecutionNode BuildExecutionNode(
            ExecutionNode node,
            IGraphType resolvedType,
            Field nodeField,
            FieldType fieldType);
        void SetArrayItemNodes(ExecutionContext context, ArrayExecutionNode parent);
    }
}
