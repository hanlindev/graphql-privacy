using GraphQL.Types;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class SampleSchema : Schema
    {
        public SampleSchema(SampleQuery query, SampleMutation mutation, IDependencyResolver resolver)
        {
            Query = query;
            Mutation = mutation;
            DependencyResolver = resolver;
        }
    }
}
