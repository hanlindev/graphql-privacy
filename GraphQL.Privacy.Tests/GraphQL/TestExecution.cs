using GraphQL.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Privacy.Tests.GraphQL
{
    public static class TestExecution
    {
        public static ExecutionNode BuildEmptyNode()
        {
            return new Mock<ExecutionNode>(null, null, null, null, new string[] { }).Object;
        }

        public static ExecutionNode BuildResultNode(object result)
        {
            var returnValue = BuildEmptyNode();
            returnValue.Result = result;
            return returnValue;
        }
    }
}
