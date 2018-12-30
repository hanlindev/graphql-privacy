using GraphQL.Types;

namespace GraphQL.Privacy.Tests.GraphQL
{
    public class Photo
    {
        public long Id { get; set; }
        public long? AlbumId { get; set; }
        public Album Album { get; set; }
    }

    public class PhotoType : ObjectGraphType<Photo>
    {
        public PhotoType()
        {
            Name = "Photo";
            Field(photo => photo.Id);
            Field(photo => photo.AlbumId);
            Field<AlbumType, Album>("Album");
        }
    }
}
