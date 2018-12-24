using GraphQL.Privacy.Sample.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class AlbumQuery : ObjectGraphType
    {
        public AlbumQuery()
        {
            Name = "AlbumQuery";
            Field<AlbumType, Album>()
                .Name("findById")
                .Argument<NonNullGraphType<IntGraphType>>("id", "Find by Id field of an Album")
                .ResolveAsync(FindByIdAsync);
        }

        private async Task<Album> FindByIdAsync(ResolveFieldContext<object> context)
        {
            var db = context.Resolve<SampleDbContext>();
            var id = context.GetArgument<long>("id");
            return await db.Albums.FindAsync(id);
        }
    }
}
