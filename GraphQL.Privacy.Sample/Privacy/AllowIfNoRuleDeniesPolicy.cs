using GraphQL.Privacy.Policies;
using GraphQL.Privacy.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Privacy
{
    public class AllowIfNoRuleDeniesPolicy<T> : ShortCircuitPolicy<T>
        where T : class
    {
        public AllowIfNoRuleDeniesPolicy(params IAuthorizationRule<T>[] rules)
        {
            Rules = rules.ToList().Concat(new List<IAuthorizationRule<T>>
            {
                new AlwaysAllowRule<T>()
            }).ToList();
        }
    }
}
