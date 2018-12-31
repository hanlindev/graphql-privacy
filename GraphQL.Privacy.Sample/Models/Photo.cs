using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Models
{
    public class Photo
    {
        public long Id { get; set; }
        public long AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
