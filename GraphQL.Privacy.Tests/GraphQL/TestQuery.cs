using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Privacy.Test.GraphQL
{
    class TestQuery : ObjectGraphType
    {
        public TestQuery()
        {
            Name = "test";
        }
    }
}
