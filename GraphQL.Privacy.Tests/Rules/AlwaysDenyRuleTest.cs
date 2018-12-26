using GraphQL.Privacy.Rules;
using GraphQL.Privacy.Test.GraphQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Privacy.Tests.Rules
{
    public class AlwaysDenyRuleTest
    {
        [Fact]
        public async Task AlwaysReturnDeny()
        {
            var rule = new AlwaysDenyRule<User>();
            var result = await rule.AuthorizeAsync(null);
            Assert.IsType<Deny>(result);
        }
    }
}
