using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Privacy
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQLPrivacy<TDB>(
            this IServiceCollection services)
            where TDB : DbContext
        {
            return services
                // Additional dependencies implemented in this package
                .AddSingleton<IHttpContextResolverService, HttpContextResolverService>()
                .AddScoped<IModelLoader, ModelLoader<TDB>>()
                .AddSingleton(typeof(ITypedResolverService<>), typeof(TypedResolverService<>))
                // Default dependencies from GraphQL.Privacy package
                .AddSingleton<IExecutionStrategyHelpers, ExecutionStrategyHelpers>()
                .AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService))
                .AddSingleton<IDocumentExecuter, AuthorizationEnabledDocumentExecuter>();
        }
    }
}
