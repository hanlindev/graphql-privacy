using GraphQL.Builders;
using GraphQL.Privacy.Sample.Models;
using GraphQL.Privacy.Sample.Models.Abstractions;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                // This will filter edges whose nodes are denied access.
                .AuthorizeConnection<AlbumType, Album, User>()
                .Name("albums")
                .ResolveAsync(ResolveAlbumsConnection);
        }

        private async Task<object> ResolveAlbumsConnection(ResolveConnectionContext<User> context)
        {
            var db = context.Resolve<SampleDbContext, User>();
            var ownerId = context.Source.Id;
            Expression<Func<Album, bool>> matchUserPredicate = album => album.UserId == ownerId;
            var totalCount = db.Albums.Where(matchUserPredicate).Count();

            // +1 to determine if there is more
            var first = context.First.GetValueOrDefault(10) + 1;
            var after = new IDCursor<Album>();
            after.Decode(context.After);

            IQueryable<Album> query;
            Expression<Func<Album, object>> orderBy = album => album.Id;
            if (after.IsValid)
            {
                query = db.Albums.Where(matchUserPredicate.And(after.Comparator)).OrderBy(orderBy);
            }
            else
            {
                query = db.Albums.Where(matchUserPredicate).OrderBy(orderBy);
            }
            var entries = await query.ToListAsync();
            var hasMore = entries.Count() == first;
            var returningEntries = entries.Take(first - 1);
            var endCursor = new IDCursor<Album>();
            endCursor.RestoreFromEntry(returningEntries.LastOrDefault());


            return new Connection<Album>
            {
                TotalCount = totalCount,
                PageInfo = new PageInfo
                {
                    HasNextPage = hasMore,
                    EndCursor = endCursor.IsValid ? endCursor.Encode() : null
                },
                Edges = returningEntries.Select((Album entry) =>
                {
                    var entryCursor = new IDCursor<Album>();
                    entryCursor.RestoreFromEntry(entry);
                    return new Edge<Album>
                    {
                        Cursor = entryCursor.Encode(),
                        Node = entry
                    };
                }).ToList()
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
