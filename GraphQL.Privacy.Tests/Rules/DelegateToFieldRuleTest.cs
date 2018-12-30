using GraphQL.Privacy.Tests.GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Privacy.Tests.Rules
{
    class DelegateToFieldRuleTestFixture : IDisposable
    {
        public IComplexGraphType PhotoField { get; set; }
        public TestDbContext DbContext { get; set; }
        public FieldType AlbumFieldType { get; set; }
        public string FieldName => "album";
        public Func<Photo, long?> IDGetter => photo => photo.AlbumId;

        public DelegateToFieldRuleTestFixture(AuthorizationResult delegateAuthResult)
        {
            SetupDB();
            MockAlbumFieldType();
            MockPhoto();
        }

        private void SetupDB()
        {
            var builder = new DbContextOptions<TestDbContext>();
            // TODO use sqlite
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
