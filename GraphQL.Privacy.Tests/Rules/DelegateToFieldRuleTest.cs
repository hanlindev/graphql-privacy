using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Privacy.Tests.GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Privacy.Tests.Rules
{
    class MockExecutionStrategyHelpers : IExecutionStrategyHelpers
    {
        public ExecutionNode BuildExecutionNode(ExecutionNode node, IGraphType resolvedType, Field nodeField, FieldType fieldType)
        {
            return new ObjectExecutionNode(node, resolvedType, nodeField, fieldType, new string[] { });
        }
    }

    public class DelegateToFieldRuleTestFixture : IDisposable
    {
        public IComplexGraphType PhotoField { get; set; }
        public IAuthorizationContext<Photo> AuthContext { get; set; }
        public TestDbContext DbContext { get; set; }
        public FieldType AlbumFieldType { get; set; }
        public string FieldName => "album";
        public Func<Photo, long?> IDGetter => photo => photo.AlbumId;

        public DelegateToFieldRuleTestFixture(AuthorizationResult delegateAuthResult)
        {
            SetupDB();
            MockAuthContext();
            MockAlbumFieldType();
            MockPhoto();
        }

        private void SetupDB()
        {
            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseInMemoryDatabase("test");
            DbContext = new TestDbContext(builder.Options);
        }

        private void MockAuthContext()
        {
            var mock = new Mock<IAuthorizationContext<Photo>>();
            mock.Setup(context => context.Resolve<IExecutionStrategyHelpers>()).Returns(new MockExecutionStrategyHelpers());
            AuthContext = mock.Object;
        }

        private void MockAlbumFieldType()
        {
            AlbumFieldType = new Mock<FieldType>().Object;
        }

        private void MockPhoto()
        {
            var mock = new Mock<IComplexGraphType>();
            mock.Setup(field => field.GetField(FieldName)).Returns(AlbumFieldType);
            PhotoField = mock.Object;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }

    public class DelegateToFieldRuleTest
    {
        [Fact]
        public async Task ReturnsAllowIfDelegateRuleReturnsAllow()
        {
        }
    }
}
