using GraphQL.Types;

namespace GraphQL.Privacy.Test.GraphQL
{
    public class User
    {
        public long Id { get; set; }
    }

    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Name = "User";
            Field(user => user.Id);
        }
    }
}
