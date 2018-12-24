using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class SampleQuery : ObjectGraphType
    {
        public SampleQuery()
        {
            Name = "Query";
            Field<UserQuery>("user", resolve: context => new { });
            Field<AlbumQuery>("album", resolve: context => new { });
        }
    }
}
