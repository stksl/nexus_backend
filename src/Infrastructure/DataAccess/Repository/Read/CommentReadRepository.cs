using System.Data;
using Dapper;
using Nexus.Application;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class CommentReadRepository : ICommentReadRepository 
{
    private readonly IDbConnection _dbConnection;
    public CommentReadRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task<Comment?> GetCommentById(int id) 
    {
        const string sql = "SELECT * FROM \"Comments\" WHERE \"Id\" = @Id";

        return _dbConnection.QueryFirstOrDefaultAsync<Comment>(sql, new {Id = id});
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId, QueryObject queryObject) 
    {
        int offset = (queryObject.PageNumber - 1) * queryObject.PageSize;

        string sql = "SELECT * FROM \"Comments\" WHERE \"PostId\" = @PostId" + 
        " LIMIT @PageSize OFFSET @Offset";

        return await _dbConnection.QueryAsync<Comment>(sql, new 
        {
            PostId = postId, 
            PageSize = queryObject.PageSize, 
            Offset = offset
        });
    }
}