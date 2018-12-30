using GraphQL.Privacy.Policies;
using GraphQL.Privacy.Rules;
using GraphQL.Privacy.Sample.Models;
using GraphQL.Privacy.Sample.Privacy;
using GraphQL.Privacy.Sample.Privacy.Rules;
using GraphQL.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.GraphQL
{
    public class AlbumType : ObjectGraphType<Album>
    {
        public AlbumType()
        {
            Name = "Album";
            this.AuthorizeWith(
                new AllowIfNoRuleDeniesPolicy<Album>(
                    new AllowIfViewerIsOwnerRule<Album>(album => album.UserId),
                    new DenyIfHidden<Album>()));
            Field(album => album.Id);
            Field(album => album.Title);
            Field(album => album.IsHidden);
            Field(album => album.UserId);
            Field<UserType, User>()
                .Name("user")
                .ResolveAsync(ResolveUser);
        }

        private async Task<User> ResolveUser(ResolveFieldContext<Album> context)
        {
            var db = context.Resolve<SampleDbContext, Album>();
            return await db.Users.FindAsync(context.Source.UserId);
        }
    }
}
