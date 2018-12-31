using GraphQL.Builders;
using GraphQL.Privacy.Policies;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using System.Collections.Generic;

namespace GraphQL.Privacy
{
    public static class PrivacyMetadataExtensions
    {
        public static ConnectionBuilder<T, TSource> AuthorizeWith<T, TNode, TSource>(
            this ConnectionBuilder<T, TSource> instance, ConnectionShortCircuitPolicy<T, TNode, TSource> policy)
            where T : ObjectGraphType<TNode>
            where TNode : class
        {
            instance.FieldType.AuthorizeWith<Connection<TNode>>(policy);
            return instance;
        }

        public static ConnectionBuilder<T, TSource> AuthorizeConnection<T, TNode, TSource>(
            this ConnectionBuilder<T, TSource> instance)
            where T : ObjectGraphType<TNode>
            where TNode : class
        {
            instance.AuthorizeWith(
                new ConnectionShortCircuitPolicy<T, TNode, TSource>(
                    instance,
                    new EdgeNodeShortCircuitPolicy<T, TNode>()
                )
            );
            return instance;
        }

        public static FieldBuilder<TSource, IEnumerable<TItem>> AuthorizeList<TSource, T, TItem>(
            this FieldBuilder<TSource, IEnumerable<TItem>> field)
            where TItem : class
            where T : ObjectGraphType<TItem>
        {
            field.AuthorizeWith(
                new ListItemShortCircuitPolicy<T, TItem>()
            );
            return field;
        }
    }
}
