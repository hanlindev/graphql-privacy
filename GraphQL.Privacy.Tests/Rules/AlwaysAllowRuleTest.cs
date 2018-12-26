using GraphQL.Privacy.Rules;
using GraphQL.Privacy.Test.GraphQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Privacy.Test.Rules
{
    public class AlwaysAllowRuleTest
    {
        [Fact]
        public async Task AlwaysReturnsAllow()
        {
            var rule = new AlwaysAllowRule<User>();
            var result = await rule.AuthorizeAsync(null);
            Assert.IsType<Allow<User>>(result);
        }
    }
}
