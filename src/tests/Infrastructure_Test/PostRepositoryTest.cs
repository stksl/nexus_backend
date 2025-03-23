using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.DataAccess;

namespace Nexus.Tests;

public class PostRepositoryTest
{
    [Fact]
    public void CreatePost_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using (NexusDbContext dbContext = new NexusDbContext(builder.Options))
        {
            Post post = new Post()
            {
                Id = 1,
                Content = "some content",
                Headline = "some headline",
                UserId = 1,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now
            };

            dbContext.Add(post);

            int saved = dbContext.SaveChanges();

            Assert.Equal(1, saved);
        }
    }
    [Fact]
    public void UpdatePost_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using (NexusDbContext dbContext = new NexusDbContext(builder.Options))
        {
            var post = new Post()
            {
                Id = 1,
                Content = "some content",
                Headline = "some headline",
                UserId = 1,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now
            };
            dbContext.Add(post);
            dbContext.SaveChanges();
        }

        using (NexusDbContext dbContext = new NexusDbContext(builder.Options))
        {
            var post = dbContext.Posts.First();

            post.Headline = "Updated";

            int updated = dbContext.SaveChanges();

            Assert.Equal(1, updated);
        }
    }
    [Fact]
    public void RemovePost_Test() 
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using (NexusDbContext dbContext = new NexusDbContext(builder.Options))
        {
            var post = new Post()
            {
                Id = 1,
                Content = "some content",
                Headline = "some headline",
                UserId = 1,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now
            };
            dbContext.Add(post);
            dbContext.SaveChanges();
        }

        using (NexusDbContext dbContext = new NexusDbContext(builder.Options))
        {
            var post = dbContext.Posts.First();

            dbContext.Posts.Remove(post);

            int removed = dbContext.SaveChanges();

            Assert.Equal(1, removed);
        }
    }
}