using Microsoft.AspNetCore.Http;

namespace GraphQL.Privacy
{
    public interface IHttpContextResolverService
    {
        T Get<T>() where T : class;
        HttpContext HttpContext { get; }
    }
}
