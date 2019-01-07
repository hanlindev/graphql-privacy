using GraphQL.Privacy.Rules;
using GraphQL.Privacy.Test.GraphQL;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Privacy.Tests.Rules
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
