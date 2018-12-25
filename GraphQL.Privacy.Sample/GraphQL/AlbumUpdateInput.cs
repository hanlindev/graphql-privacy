using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class AlbumUpdateInput
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool? IsHidden { get; set; }
    }

    public class AlbumUpdateInputType : InputObjectGraphType<AlbumUpdateInput>
    {
        public AlbumUpdateInputType()
        {
            Name = "AlbumUpdateInput";
            Field(input => input.Id);
            Field<StringGraphType, string>("Title");
            Field<BooleanGraphType, bool?>("IsHidden");
        }
    }
}
