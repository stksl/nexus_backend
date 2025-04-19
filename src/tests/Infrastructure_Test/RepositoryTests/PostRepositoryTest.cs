using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Dapper;
using Nexus.Domain.Entities;
using Nexus.Application;
using Nexus.Infrastructure.DataAccess;
using Nexus.Application.Dtos;

namespace Nexus.Tests;

public class PostRepositoryTest
{
    private Mock<DbConnection> dbConnectionMock;
    public PostRepositoryTest()
    {
        dbConnectionMock = new Mock<DbConnection>();
    }
    [Fact]
    public async Task CreatePost_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using NexusDbContext dbContext = new NexusDbContext(builder.Options);

        PostRepository postRepository = new PostRepository(dbContext);

        Post post = new Post() 
        {
            Content = "some content",
            Headline = "some headline",
            DateCreated = DateTime.UtcNow,

        };

        await postRepository.AddPost(post);
        int saved = await dbContext.SaveChangesAsync();

        Assert.Equal(expected: 1, actual: saved);
    }
    [Fact]
    public async Task UpdatePost_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using NexusDbContext dbContext = new NexusDbContext(builder.Options);

        PostRepository postRepository = new PostRepository(dbContext);

        Post post = new Post() 
        {
            Content = "some content",
            Headline = "some headline",
            DateCreated = DateTime.UtcNow,

        };

        dbContext.Add(post);
        await dbContext.SaveChangesAsync();

        postRepository.UpdatePost(post);
        int updated = await dbContext.SaveChangesAsync();

        Assert.Equal(expected: 1, actual: updated);
    }
    [Fact]
    public async Task DeletePost_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using NexusDbContext dbContext = new NexusDbContext(builder.Options);

        PostRepository postRepository = new PostRepository(dbContext);

        Post post = new Post() 
        {
            Content = "some content",
            Headline = "some headline",
            DateCreated = DateTime.UtcNow,

        };

        dbContext.Add(post);
        await dbContext.SaveChangesAsync();

        postRepository.RemovePost(post);
        int updated = await dbContext.SaveChangesAsync();

        Assert.Equal(expected: 1, actual: updated);
    }
    [Fact]
    public async Task GetPostById_Test() 
    {
        PostResponse expected = new PostResponse() 
        {
            Headline = "some other headline",
            LikeCount = 23,
            Tags = ["discussion", "js"]
        };

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryFirstOrDefaultAsync<PostResponse>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        PostReadRepository postReadRepository = new PostReadRepository(dbConnectionMock.Object);
        PostResponse? actual = await postReadRepository.GetPostWithLikesById(1);

        Assert.Equal(expected: expected.Headline, actual: actual?.Headline);
    }

    [Fact]
    public async Task GetPostByUser_Test() 
    {
        IEnumerable<PostResponse> expected = [
            new PostResponse() 
            {
                Headline = "headline 1",
                LikeCount = 20,
                Tags = ["js", ".net"]
            },
            new PostResponse() 
            {
                Headline = "headline 2",
                LikeCount = 15,
                Tags = ["tag1", "tag2", "tag3"]
            }
        ];

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryAsync<PostResponse>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        PostReadRepository postReadRepository = new PostReadRepository(dbConnectionMock.Object);

        IEnumerable<PostResponse> actual = await postReadRepository.GetPostsWithLikesByUser(1, new QueryObject<PostResponse>());

        Assert.Equal(expected, actual, EqualityComparer<PostResponse>.Create((left, right) => left?.Headline == right?.Headline));
    }
}