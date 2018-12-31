using GraphQL.Privacy.Sample.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class PhotoQuery : ObjectGraphType
    {
        public PhotoQuery()
        {
            Name = "PhotoQuery";
            Field<PhotoType, Photo>()
                .Name("findById")
                .Argument<NonNullGraphType<IntGraphType>>("id", "Id of the Photo model")
                .ResolveAsync(ResolvePhoto);

        }

        private async Task<Photo> ResolvePhoto(ResolveFieldContext<object> context)
        {
            var db = context.Resolve<SampleDbContext>();
            var id = context.GetArgument<long>("id");
            return await db.Photos.FindAsync(id);
        }
    }
}
