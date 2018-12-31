using Microsoft.AspNetCore.Http;

namespace GraphQL.Privacy
{
    public class TypedResolverService<T> : HttpContextResolverService, ITypedResolverService<T> where T : class
    {
        public TypedResolverService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        { }

        public T Get()
        {
            return Get<T>();
        }
    }
}
