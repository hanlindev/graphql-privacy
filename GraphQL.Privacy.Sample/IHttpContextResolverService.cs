using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample
{
    public interface IHttpContextResolverService
    {
        T Get<T>() where T : class;
        HttpContext HttpContext { get; }
    }
}
