using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Models.Abstractions
{
    public class IDCursorFields : ISupportIDCursor
    {
        public long Id { get; set; }
    }

    public class IDCursor<T> : PagingCursor<IDCursorFields, T>
        where T : ISupportIDCursor
    {
        public override Expression<Func<T, bool>> Comparator => (T instance) => instance.Id > Fields.Id;

        public override void RestoreFromEntry(T target)
        {
            if (target != null)
            {
                Fields = new IDCursorFields { Id = target.Id };
            }
        }
    }
}
