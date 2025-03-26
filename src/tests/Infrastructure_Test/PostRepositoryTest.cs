using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Dapper;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.DataAccess;

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
    public async Task GetPostById_Test() 
    {
        Post expected = new Post() 
        {
            Headline = "some other headline",
        };

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryFirstOrDefaultAsync<Post>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        PostReadRepository postReadRepository = new PostReadRepository(dbConnectionMock.Object);
        Post? actual = await postReadRepository.GetPostById(1);

        Assert.NotNull(actual);

        Assert.Equal(expected: expected.Headline, actual: actual.Headline);
    }

    [Fact]
    public async Task GetPostByUser_Test() 
    {
        IEnumerable<Post> expected = [
            new Post() 
            {
                Headline = "headline 1",
            },
            new Post() 
            {
                Headline = "headline 2"
            }
        ];

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryAsync<Post>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        PostReadRepository postReadRepository = new PostReadRepository(dbConnectionMock.Object);

        IEnumerable<Post> actual = await postReadRepository.GetPostsByUser(1);

        Assert.Equal(expected, actual, EqualityComparer<Post>.Create((left, right) => left?.Headline == right?.Headline));
    }
}