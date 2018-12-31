using GraphQL.Privacy.Sample.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GraphQL.Privacy.Sample
{
    public class SampleDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User { Id = 1, Name = "User1" },
                new User { Id = 2, Name = "User2" },
                new User { Id = 3, Name = "User3" });
            builder.Entity<Album>().HasData(
                new Album { Id = 1, Title = "Album 1", IsHidden = false, UserId = 1 },
                new Album { Id = 2, Title = "Album 2", IsHidden = false, UserId = 1 },
                new Album { Id = 3, Title = "Album 3", IsHidden = false, UserId = 1 },
                new Album { Id = 4, Title = "Album 4", IsHidden = false, UserId = 2 },
                new Album { Id = 5, Title = "Album 5", IsHidden = true, UserId = 2 },
                new Album { Id = 6, Title = "Album 6", IsHidden = false, UserId = 2 });
            builder.Entity<Photo>().HasData(
                new Photo { Id = 1, AlbumId = 1 },
                new Photo { Id = 2, AlbumId = 2 },
                new Photo { Id = 3, AlbumId = 3 },
                new Photo { Id = 4, AlbumId = 4 },
                new Photo { Id = 5, AlbumId = 5 },
                new Photo { Id = 6, AlbumId = 6 });
        }
    }
}
