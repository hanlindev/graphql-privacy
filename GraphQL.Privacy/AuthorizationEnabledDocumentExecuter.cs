using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Instrumentation;
using GraphQL.Language.AST;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using static GraphQL.Execution.ExecutionHelper;
using ExecutionContext = GraphQL.Execution.ExecutionContext;

namespace GraphQL.Privacy
{
    public class AuthorizationEnabledDocumentExecuter : DocumentExecuter
    {
        protected override IExecutionStrategy SelectExecutionStrategy(ExecutionContext context)
        {
            switch (context.Operation.OperationType)
            {
                case OperationType.Query:
                    return new ParallelExecutionWithAuthorizationStrategy();

                case OperationType.Mutation:
                    return new SerialExecutionWithAuthorizationStrategy();

                case OperationType.Subscription:
                    return new SubscriptionExecutionStrategy();

                default:
                    throw new InvalidOperationException($"Unexpected OperationType {context.Operation.OperationType}");
            }
        }
    }
}