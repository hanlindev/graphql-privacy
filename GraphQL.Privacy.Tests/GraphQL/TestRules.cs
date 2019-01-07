using GraphQL.Execution;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Tests.GraphQL
{
    public class TestRules<T> : IDisposable
    {
        public static Allow<T> Allow()
        {
            return new Allow<T>("Allowed by test");
        }

        public static Deny Deny()
        {
            return new Deny("Denied by test");
        }

        public static Skip Skip()
        {
            return new Skip("Skipped by test");
        }

        public IProvideClaimsPrincipal UserContext { get; private set; }
        public ExecutionNode SourceNode { get; private set; }
        public ExecutionContext SourceContext { get; private set; }
        public IAuthorizationContext<T> AuthContext { get; private set; }

        public TestRules(T subject)
        {
            UserContext = MockUserContext();
            SourceNode = MockSourceNode(subject);
            SourceContext = MockSourceContext();
            AuthContext = MockAuthContext(subject);
        }

        private IProvideClaimsPrincipal MockUserContext()
        {
            var mock = new Mock<IProvideClaimsPrincipal>();
            mock.Setup(userContext => userContext.User).Returns(new ClaimsPrincipal());
            return mock.Object;
        }

        private ExecutionNode MockSourceNode(T result)
        {
            return TestExecution.BuildResultNode(result);
        }

        private ExecutionContext MockSourceContext()
        {
            return new ExecutionContext()
            {
                UserContext = UserContext
            };
        }

        private IAuthorizationContext<T> MockAuthContext(T subject)
        {
            var mockAuthContext = new Mock<IAuthorizationContext<T>>();
            mockAuthContext.Setup(context => context.Subject).Returns(subject);
            mockAuthContext.Setup(context => context.ExecutionContext).Returns(SourceContext);
            mockAuthContext.Setup(context => context.ExecutionNode).Returns(SourceNode);
            return mockAuthContext.Object;
        }

        private IAuthorizationRule<T> MockRule(AuthorizationResult result)
        {
            var mockRule = new Mock<IAuthorizationRule<T>>();
            mockRule.Setup(rule => rule.AuthorizeAsync(It.IsAny<IAuthorizationContext<T>>()))
                .Returns(Task.FromResult(result));
            return mockRule.Object;
        }

        public IAuthorizationRule<T> AllowRule()
        {
            return MockRule(Allow());
        }

        public IAuthorizationRule<T> DenyRule()
        {
            return MockRule(Deny());
        }

        public IAuthorizationRule<T> SkipRule()
        {
            return MockRule(Skip());
        }

        public void Dispose()
        {
        }
    }
}
