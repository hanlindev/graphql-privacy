using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample
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
