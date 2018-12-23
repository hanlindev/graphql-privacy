using GraphQL.Privacy.Sample.Models;
using GraphQL.Types;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class UserQuery : ObjectGraphType<User>
    {
        public UserQuery()
        {
            Field(u => u.Id);
            Field(u => u.Name);
        }
    }
}
