using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Privacy
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQLPrivacy(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IExecutionStrategyHelpers, ExecutionStrategyHelpers>()
                .AddSingleton<IHttpContextResolverService, HttpContextResolverService>()
                .AddSingleton(typeof(ITypedResolverService<>), typeof(TypedResolverService<>))
                .AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService))
                .AddSingleton<IDocumentExecuter, AuthorizationEnabledDocumentExecuter>();
        }
    }
}
