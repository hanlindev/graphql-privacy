using GraphQL.Types;

namespace GraphQL.Privacy.Sample
{
    public static class ResolveFieldContextExtensions
    {
        public static T Resolve<T, TSource>(this ResolveFieldContext<TSource> context)
        {
            var schema = context.Schema as Schema;
            return schema.DependencyResolver.Resolve<ITypedResolverService<T>>().Get();
        }

        public static T Resolve<T>(this ResolveFieldContext<object> context)
        {
            return context.Resolve<T, object>();
        }
    }
}
