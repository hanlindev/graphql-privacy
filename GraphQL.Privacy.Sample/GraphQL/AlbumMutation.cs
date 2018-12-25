using GraphQL.Privacy.Sample.Models;
using GraphQL.Privacy.Sample.Privacy;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class AlbumMutation : ObjectGraphType
    {
        public AlbumMutation()
        {
            Field<AlbumType, Album>()
                // Note: this will fail because EFCore.Sqlite provider does not support ambient transaction.
                // If you'd like to try mutation privacy, you can change the provider to one that supports
                // ambient transactions, for example PostgreSQL.
                .AuthorizeWith(new AllowIfViewerIsOwnerOrDenyPolicy<Album>(album => album.UserId))
                .Name("update")
                .Argument<AlbumUpdateInputType>("input", "Fields that can be updated on an album")
                .ResolveAsync(UpdateAlbum);
        }

        private async Task<Album> UpdateAlbum(ResolveFieldContext<object> context)
        {
            var db = context.Resolve<SampleDbContext>();
            var input = context.GetArgument<AlbumUpdateInput>("input");
            var album = await db.Albums.FindAsync(input.Id);
            if (!string.IsNullOrEmpty(input.Title))
            {
                album.Title = input.Title;
            }

            if (input.IsHidden is bool isHidden)
            {
                album.IsHidden = isHidden;
            }

            db.Albums.Update(album);
            await db.SaveChangesAsync();
            return album;
        }
    }
}
