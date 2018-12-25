using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class SampleMutation : ObjectGraphType
    {
        public SampleMutation()
        {
            Name = "Mutation";
            Field<AlbumMutation>("album", resolve: context => new { });
        }
    }
}
