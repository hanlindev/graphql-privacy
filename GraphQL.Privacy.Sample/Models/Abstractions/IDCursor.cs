using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Models.Abstractions
{
    public class IDCursor<T> : PagingCursor<ISupportIDCursor, T>
        where T : ISupportIDCursor
    {
        public override Expression<Func<T, bool>> Comparator => (T instance) => instance.Id > Fields.Id;

        public override void RestoreFromEntry(T target)
        {
            if (target != null)
            {
                Fields = target;
            }
        }
    }
}
