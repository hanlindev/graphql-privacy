using GraphQL.Privacy.Rules;
using GraphQL.Privacy.Sample.Models;
using GraphQL.Privacy.Sample.Privacy;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class PhotoMutation : ObjectGraphType
    {
        public PhotoMutation(ITypedResolverService<PhotoType> photoTypeResolver)
        {
            Name = "PhotoMutation";
            Field<PhotoType, Photo>("create")
                .Type(photoTypeResolver.Get())
                .AuthorizeWith(
                    new DenyIfNoRuleAllowsPolicy<Photo>(
                        new DelegateToFieldRule<PhotoType, Photo, AlbumType, Album, long>(
                            this,
                            "create",
                            "album",
                            photo => photo.AlbumId)))
                .Argument<NonNullGraphType<IntGraphType>>("id", "Id of the photo")
                .Argument<NonNullGraphType<IntGraphType>>("albumId", "Id of the album this photo belongs to")
                .ResolveAsync(CreatePhotoAsync);
        }

        private async Task<Photo> CreatePhotoAsync(ResolveFieldContext<object> context)
        {
            var db = context.Resolve<SampleDbContext>();
            var id = context.GetArgument<long>("id");
            var albumId = context.GetArgument<long>("albumId");
            var newPhoto = new Photo { Id = id, AlbumId = albumId };
            db.Add(newPhoto);
            await db.SaveChangesAsync();
            return newPhoto;
        }
    }
}
