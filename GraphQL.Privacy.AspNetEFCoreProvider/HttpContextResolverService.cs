using Microsoft.AspNetCore.Http;

namespace GraphQL.Privacy
{
    public class HttpContextResolverService : IHttpContextResolverService
    {
        private IHttpContextAccessor contextAccessor;

        public HttpContextResolverService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public HttpContext HttpContext
        {
            get
            {
                return contextAccessor.HttpContext;
            }
        }

        public T Get<T>() where T : class
        {
            return HttpContext.RequestServices.GetService(typeof(T)) as T;
        }
    }
}
