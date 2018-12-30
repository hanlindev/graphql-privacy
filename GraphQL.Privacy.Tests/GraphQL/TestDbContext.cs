using GraphQL.Privacy.Test.GraphQL;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Privacy.Tests.GraphQL
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User { Id = 1 },
                new User { Id = 2 },
                new User { Id = 3 });
            builder.Entity<Album>().HasData(
                new Album { Id = 1, UserId = 1 },
                new Album { Id = 2, UserId = 1 },
                new Album { Id = 3, UserId = 1 },
                new Album { Id = 4, UserId = 2 });
        }
    }
}
