using GraphQL.Privacy.Policies;
using GraphQL.Privacy.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Privacy
{
    public class DenyIfNoRuleAllowsPolicy<T> : ShortCircuitPolicy<T>
        where T : class
    {
        public DenyIfNoRuleAllowsPolicy(params IAuthorizationRule<T>[] rules)
        {
            Requirements = rules.ToList().Concat(new List<IAuthorizationRule<T>>
            {
                new AlwaysDenyRule<T>()
            }).ToList();
        }
    }
}
