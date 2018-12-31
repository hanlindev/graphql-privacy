using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Builders;
using GraphQL.Execution;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;

namespace GraphQL.Privacy
{
    public static class AuthorizationMetadataExtensions
    {
        public static readonly string PolicyMetadataKey = "HTAuthorizationPolicies";

        public static void AuthorizeWith<TReturnType>(this IProvideMetadata instance, IAuthorizationPolicy<TReturnType> policy)
        {
            var hasExistingPolicy = instance.HasMetadata(PolicyMetadataKey);
            if (hasExistingPolicy) {
                throw new OverwritePolicyException();
            }
            instance.Metadata[PolicyMetadataKey] = policy;
        }

        public static FieldBuilder<T, TReturnType> AuthorizeWith<T, TReturnType>(
            this FieldBuilder<T, TReturnType> instance, IAuthorizationPolicy<TReturnType> policy)
        {
            instance.FieldType.AuthorizeWith<TReturnType>(policy);
            return instance;
        }

        public static IAuthorizationPolicy<TReturn> GetPolicy<TReturn>(this IProvideMetadata instance)
        {
            return instance.GetMetadata<IAuthorizationPolicy<TReturn>>(PolicyMetadataKey);
        }

        public static async Task<AuthorizationResult> AuthorizeAsync<TSource>(
            this IProvideMetadata instance, ExecutionContext context, ExecutionNode node)
        {
            var policy = instance.GetPolicy<TSource>();
            if (policy == null) {
                return new Skip(instance);
            }

            var copy = policy.BuildCopy(context, node);
            return await copy.AuthorizeAsync();
        }
    }
}