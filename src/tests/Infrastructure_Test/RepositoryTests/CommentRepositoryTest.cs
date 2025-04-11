using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Dapper;
using Nexus.Domain.Entities;
using Nexus.Application;
using Nexus.Infrastructure.DataAccess;

namespace Nexus.Tests;

public class CommentRepositoryTest
{
    private Mock<DbConnection> dbConnectionMock;
    public CommentRepositoryTest()
    {
        dbConnectionMock = new Mock<DbConnection>();
    }
    [Fact]
    public async Task CreateComment_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using NexusDbContext dbContext = new NexusDbContext(builder.Options);

        CommentRepository commentRepository = new CommentRepository(dbContext);

        Comment comment = new Comment() 
        {
            Content = "some content",
            PostId = 1,
            UserId = 2,
            DateCreated = DateTime.UtcNow,
        };

        await commentRepository.AddComment(comment);
        int saved = await dbContext.SaveChangesAsync();

        Assert.Equal(expected: 1, actual: saved);
    }
    [Fact]
    public async Task UpdateComment_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using NexusDbContext dbContext = new NexusDbContext(builder.Options);

        CommentRepository commentRepository = new CommentRepository(dbContext);

        Comment comment = new Comment() 
        {
            Content = "some content",
            PostId = 1,
            UserId = 2,
            DateCreated = DateTime.UtcNow,
        };

        dbContext.Add(comment);
        await dbContext.SaveChangesAsync();

        commentRepository.UpdateComment(comment);
        int updated = await dbContext.SaveChangesAsync();

        Assert.Equal(expected: 1, actual: updated);
    }
    [Fact]
    public async Task DeleteComment_Test()
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(RandomDbName);

        using NexusDbContext dbContext = new NexusDbContext(builder.Options);

        CommentRepository commentRepository = new CommentRepository(dbContext);

        Comment comment = new Comment() 
        {
            Content = "some content",
            PostId = 1,
            UserId = 2,
            DateCreated = DateTime.UtcNow,
        };

        dbContext.Add(comment);
        await dbContext.SaveChangesAsync();

        commentRepository.RemoveComment(comment);
        int updated = await dbContext.SaveChangesAsync();

        Assert.Equal(expected: 1, actual: updated);
    }
    [Fact]
    public async Task GetCommentById_Test() 
    {
        Comment expected = new Comment() 
        {
            Content = "some comment content",
        };

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryFirstOrDefaultAsync<Comment>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        CommentReadRepository commentReadRepository = new CommentReadRepository(dbConnectionMock.Object);
        Comment? actual = await commentReadRepository.GetCommentWithLikesById(1);

        Assert.Equal(expected: expected.Content, actual: actual?.Content);
    }

    [Fact]
    public async Task GetCommentsByPostId_Test() 
    {
        IEnumerable<Comment> expected = [
            new Comment() 
            {
                Content = "content 1",
            },
            new Comment() 
            {
                Content = "content 2"
            }
        ];

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryAsync<Comment>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        CommentReadRepository commentReadRepository = new CommentReadRepository(dbConnectionMock.Object);

        IEnumerable<Comment> actual = await commentReadRepository.GetCommentsWithLikesByPostId(1, new QueryObject());

        Assert.Equal(expected, actual, EqualityComparer<Comment>.Create((left, right) => left?.Content == right?.Content));
    }
}