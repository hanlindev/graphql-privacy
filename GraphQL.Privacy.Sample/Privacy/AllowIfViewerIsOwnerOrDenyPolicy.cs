using GraphQL.Privacy.Sample.Privacy.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Privacy
{
    public class AllowIfViewerIsOwnerOrDenyPolicy<T> : DenyIfNoRuleAllowsPolicy<T>
        where T : class
    {
        public AllowIfViewerIsOwnerOrDenyPolicy(Func<T, long?> getOwnerId)
            : base(new AllowIfViewerIsOwnerRule<T>(getOwnerId))
        {
        }
    }
}
