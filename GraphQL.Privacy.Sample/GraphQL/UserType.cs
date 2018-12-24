using GraphQL.Builders;
using GraphQL.Privacy.Sample.Models;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Name = "User";
            Field(u => u.Id);
            Field(u => u.Name);
            Field<ListGraphType<AlbumType>, IEnumerable<Album>>()
                .AuthorizeList<User, AlbumType, Album>()
                .Name("allAlbums")
                .ResolveAsync(ResolveAllAlbums);
            Connection<AlbumType>()
                .Name("albums")
                .ResolveAsync(ResolveAlbumsConnection);
        }

        private async Task<object> ResolveAlbumsConnection(ResolveConnectionContext<User> context)
        {
            // TODO generate the connection
            return new Connection<Album>
            {
                TotalCount = 0 // TODO
            };
        }

        private async Task<IEnumerable<Album>> ResolveAllAlbums(ResolveFieldContext<User> context)
        {
            var userId = context.Source.Id;
            var db = context.Resolve<SampleDbContext, User>();
            return await db.Albums.Where(album => album.UserId == userId).ToListAsync();
        }
    }
}
