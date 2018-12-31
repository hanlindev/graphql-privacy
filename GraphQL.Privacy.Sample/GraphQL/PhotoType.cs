using GraphQL.Privacy.Policies;
using GraphQL.Privacy.Rules;
using GraphQL.Privacy.Sample.Models;
using GraphQL.Privacy.Sample.Privacy;
using GraphQL.Types;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class PhotoType : ObjectGraphType<Photo>
    {
        public PhotoType()
        {
            Name = "Photo";
            this.AuthorizeWith(
                new DenyIfNoRuleAllowsPolicy<Photo>(
                    new DelegateToFieldRule<PhotoType, Photo, AlbumType, Album, long>(this, "album", photo => photo.AlbumId)));
            Field(photo => photo.Id);
            Field(photo => photo.AlbumId);
            Field<AlbumType, Album>()
                .Name("album")
                .ResolveAsync(ResolveAlbum);
        }

        private async Task<Album> ResolveAlbum(ResolveFieldContext<Photo> context)
        {
            var db = context.Resolve<SampleDbContext, Photo>();
            var albumId = context.Source.AlbumId;
            return await db.Albums.FindAsync(albumId);
        }
    }
}
