# GraphQL Privacy

Control what a user can see at the GraphQL level.

## Installation

* Base package
    You can install the latest version via [NuGet](https://www.nuget.org/packages/GraphQL.Privacy/)

    `PM> Install-Package GraphQL.Privacy`

    Or

    `$ dotnet add package GraphQL.Privacy`

* Dependency providers
    The library requires facilities that vary with the choice of MVC and ORM frameworks. For the "AspNet + EF" core stack,
    the dependency provider is provided here. You can install it via [NuGet](https://www.nuget.org/packages/GraphQL.Privacy.AspNetEFCoreProvider/)

    `PM> Install-Package GraphQL.Privacy.AspNetEFCoreProvider`

    Or

    `$ dotnet add package GraphQL.Privacy.AspNetEFCoreProvider`

## Usage

You can define privacy policies on a graph type:
```csharp
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
```

You can define policies on a field, too:
```csharp
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

	// ...
}
```

Please refer to the GraphQL.Privacy.Sample package for more examples.

## If you don't use the AspNet + EF Core combo

Please refer to the `GraphQL.Privacy.AspNetEFCoreProvider` project.
You will need to implement those interfaces and inject them the same way as that project.

## When should you use this library?

You can use this library if your authorization rules depend on the returned objects, i.e. you need
do post-resolution authorization. For example, in the Sample project. The `AlbumType` would normally 
allow any user to see. However if one album is disabled, it would only allow the owner to see. You 
can see that this syntax is really flexible and allows very complicated checks.

In addition, the library treats authorization failure the same way as being not found, i.e. the
field would return a null. This also applies to list items so you will potentially see null in
a list. If you want to remove the nulls, you can use the `ListItemShortCircuitPolicy` to
post-process the list items and remove null values. If the field is a `Connection`, you should
use `ConnectionShortCircuitPolicy`, instead.

## Future plans

I use this package in my own project so I will add more functions, policies or rules when I see
the need. In addition, the following items are planned items I'm working on at this moment:

* Implement tests for `ListItemShortCircuitPolicy` and `ConnectionShortCircuitPolicy`.
