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

    public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId) 
    {
        const string sql = "SELECT * FROM \"Comments\" WHERE \"PostId\" = @PostId";

        return await _dbConnection.QueryAsync<Comment>(sql, new {PostId = postId});
    }
}