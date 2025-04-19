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
        CommentResponse expected = new CommentResponse() 
        {
            Content = "some comment content",
            LikesCount = 1
        };

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryFirstOrDefaultAsync<CommentResponse>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        CommentReadRepository commentReadRepository = new CommentReadRepository(dbConnectionMock.Object);
        CommentResponse? actual = await commentReadRepository.GetCommentWithLikesById(1);

        Assert.Equal(expected: expected.Content, actual: actual?.Content);
    }

    [Fact]
    public async Task GetCommentsByPostId_Test() 
    {
        IEnumerable<CommentResponse> expected = [
            new CommentResponse() 
            {
                Content = "content 1",
            },
            new CommentResponse() 
            {
                Content = "content 2"
            }
        ];

        dbConnectionMock.SetupDapperAsync(dbConnection => 
            dbConnection.QueryAsync<CommentResponse>(It.IsAny<string>(), null, null, null, null))
        .ReturnsAsync(expected);

        CommentReadRepository commentReadRepository = new CommentReadRepository(dbConnectionMock.Object);

        IEnumerable<CommentResponse> actual = await commentReadRepository.GetCommentsWithLikesByPostId(1, new QueryObject<CommentResponse>());

        Assert.Equal(expected, actual, EqualityComparer<CommentResponse>.Create((left, right) => left?.Content == right?.Content));
    }
}