using GraphQL.Privacy.Policies;
using GraphQL.Privacy.Tests.GraphQL;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Privacy.Tests.Policies
{
    public class ShortCircuitPolicyTest
    {
        [Fact]
        public async Task ReturnsAllowIfFirstDecisionIsAllow()
        {
            using (var rules = new TestRules<Album>(new Album()))
            {
                var subject = new ShortCircuitPolicy<Album>(
                    rules.SkipRule(),
                    rules.AllowRule(),
                    rules.DenyRule());
                var copy = subject.BuildCopy(rules.SourceContext, rules.SourceNode);
                var result = await copy.AuthorizeAsync();
                Assert.IsType<Allow<Album>>(result);
            }
        }

        [Fact]
        public async Task ReturnsDenyIfFirstDecisionIsDeny()
        {
            using (var rules = new TestRules<Album>(new Album()))
            {
                var subject = new ShortCircuitPolicy<Album>(
                    rules.SkipRule(),
                    rules.DenyRule(),
                    rules.AllowRule());
                var copy = subject.BuildCopy(rules.SourceContext, rules.SourceNode);
                var result = await copy.AuthorizeAsync();
                Assert.IsType<Deny>(result);
            }
        }

        [Fact]
        public async Task ReturnsSkipIfNoDecisionIsMade()
        {
            using (var rules = new TestRules<Album>(new Album()))
            {
                var subject = new ShortCircuitPolicy<Album>(
                    rules.SkipRule());
                var copy = subject.BuildCopy(rules.SourceContext, rules.SourceNode);
                var result = await copy.AuthorizeAsync();
                Assert.IsType<Skip>(result);
            }
        }
    }
}
