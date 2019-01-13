using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Privacy.Rules;
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
        private ExecutionNode _node;

        public MockExecutionStrategyHelpers(ExecutionNode result)
        {
            _node = result;
        }
        public ExecutionNode BuildExecutionNode(ExecutionNode node, IGraphType resolvedType, Field nodeField, FieldType fieldType)
        {
            return _node;
        }

        public void SetArrayItemNodes(ExecutionContext context, ArrayExecutionNode parent)
        {
            // TODO
        }
    }

    class MockModelLoader : IModelLoader
    {
        public Task<T> FindAsync<T, TID>(TID id)
            where T : class
        {
            return Task.FromResult(new Mock<T>().Object);
        }
    }

    public class DelegateToFieldRuleTestContext : IDisposable
    {
        public IComplexGraphType PhotoField { get; set; }
        public IComplexGraphType AlbumType { get; set; }
        public IAuthorizationContext<Photo> AuthContext { get; set; }
        public TestDbContext DbContext { get; set; }
        public FieldType PhotoFieldType { get; set; }
        public FieldType AlbumFieldType { get; set; }
        public string FieldName => "album";
        public Func<Photo, long?> IDGetter => photo => photo.AlbumId;
        public ExecutionNode PhotoNode { get; set; }
        public ExecutionNode DelegateNode { get; set; }

        public DelegateToFieldRuleTestContext(AuthorizationResult delegateAuthResult)
        {
            SetupDB();
            MockExecutionNodes();
            MockAlbumFieldType();
            MockPhoto();
            MockAlbum(delegateAuthResult);
            MockAuthContext();
        }

        private void SetupDB()
        {
            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseInMemoryDatabase("test");
            DbContext = new TestDbContext(builder.Options);
        }

        private void MockExecutionNodes()
        {
            PhotoNode = new Mock<ExecutionNode>(null, null, null, null, new string[] { }).Object;
            DelegateNode = new Mock<ExecutionNode>(PhotoNode, null, null, null, new string[] { }).Object;
        }

        private void MockAuthContext()
        {
            var mock = new Mock<IAuthorizationContext<Photo>>();
            mock.Setup(context => context.Resolve<IExecutionStrategyHelpers>()).Returns(new MockExecutionStrategyHelpers(DelegateNode));
            mock.Setup(context => context.Subject).Returns(new Photo { Id = 1, AlbumId = 1 });
            mock.Setup(context => context.Resolve<IModelLoader>()).Returns(new MockModelLoader());
            mock.Setup(context => context.ExecutionNode).Returns(PhotoNode);
            mock.Setup(context => context.Resolve<IComplexGraphType>()).Returns(AlbumType);
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
            PhotoFieldType = new Mock<FieldType>().Object;
        }

        private void MockAlbum(AuthorizationResult result)
        {
            var delegateAuthMock = new Mock<IAuthorizationPolicy<Album>>();
            delegateAuthMock.Setup(policy => policy.AuthorizeAsync()).Returns(Task.FromResult(result));
            delegateAuthMock.Setup(policy => policy.BuildCopy(null, DelegateNode)).Returns(delegateAuthMock.Object);
            var mock = new Mock<IComplexGraphType>();
            mock.Setup(field => field.GetMetadata<IAuthorizationPolicy<Album>>(AuthorizationMetadataExtensions.PolicyMetadataKey, null)).Returns(delegateAuthMock.Object);
            AlbumType = mock.Object;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }

    public class DelegateToFieldRuleTest
    {
        [Fact]
        public async Task ReturnsAllowIfDelegateRuleReturnsAllowOnSourceType()
        {
            using (var context = new DelegateToFieldRuleTestContext(new Allow<Album>("Allowed by test")))
            {
                var rule = new DelegateToFieldRule<IComplexGraphType, Photo, IComplexGraphType, Album, long>(
                    context.PhotoField,
                    "album",
                    photo => photo.AlbumId);
                var result = await rule.AuthorizeAsync(context.AuthContext);
                Assert.IsType<Allow<Photo>>(result);
            }
        }

        [Fact]
        public async Task ReturnsDenyIfDelegateRuleReturnsDeny()
        {
            using (var context = new DelegateToFieldRuleTestContext(new Deny("Denied by test")))
            {
                var rule = new DelegateToFieldRule<IComplexGraphType, Photo, IComplexGraphType, Album, long>(
                    context.PhotoField,
                    "album",
                    photo => photo.AlbumId);
                var result = await rule.AuthorizeAsync(context.AuthContext);
                Assert.IsType<Deny>(result);
            }
        }

        [Fact]
        public async Task ReturnsSkipIfDelegateRuleReturnsSkip()
        {
            using (var context = new DelegateToFieldRuleTestContext(new Skip("Skipped by test")))
            {
                var rule = new DelegateToFieldRule<IComplexGraphType, Photo, IComplexGraphType, Album, long>(
                    context.PhotoField,
                    "album",
                    photo => photo.AlbumId);
                var result = await rule.AuthorizeAsync(context.AuthContext);
                Assert.IsType<Skip>(result);
            }
        }
    }
}
