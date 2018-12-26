using GraphQL.Types;

namespace GraphQL.Privacy.Test.GraphQL
{
    class TestSchema : Schema
    {
        public TestSchema()
        {
            Query = new TestQuery();
        }
    }
}
