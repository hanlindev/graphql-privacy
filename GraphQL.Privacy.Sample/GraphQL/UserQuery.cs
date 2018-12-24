using GraphQL.Privacy.Sample.Models;
using GraphQL.Types;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery()
        {
            Field<UserType, User>()
                .Name("findById")
                .Argument<NonNullGraphType<IntGraphType>>("id", "The Id of the user model")
                .ResolveAsync(FindByIdAsync);
        }

        private async Task<User> FindByIdAsync(ResolveFieldContext<object> context)
        {
            var db = context.Resolve<SampleDbContext>();
            return await db.Users.FindAsync(context.GetArgument<long>("id"));
        }
    }
}
