using GraphQL.Privacy.Test.GraphQL;
using GraphQL.Types;

namespace GraphQL.Privacy.Tests.GraphQL
{
    public class Album
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }

    public class AlbumType : ObjectGraphType<Album>
    {
        public AlbumType()
        {
            Name = "Album";
            Field(album => album.Id);
            Field(album => album.UserId);
            Field<UserType, User>("User");
        }
    }
}
