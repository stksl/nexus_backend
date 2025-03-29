using System.Data;
using System.Linq.Expressions;
using Dapper;
using Nexus.Application;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostReadRepository : IPostReadRepository
{
    private readonly IDbConnection _dbConnection;
     
    public PostReadRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task<Post?> GetPostById(int id)
    {
        const string sql = "SELECT * FROM \"Posts\" WHERE \"Id\" = @Id";

        return _dbConnection.QueryFirstOrDefaultAsync<Post>(sql, new {Id = id});
    }

    public Task<IEnumerable<Post>> GetPostsByUser(int userId, QueryObject queryObject)
    {
        int offset = (queryObject.PageNumber - 1) * queryObject.PageSize;

        string sql = "SELECT * FROM \"Posts\" WHERE \"UserId\" = @UserId" + 
        " LIMIT @PageSize OFFSET @Offset";

        return _dbConnection.QueryAsync<Post>(sql, new 
        {
            UserId = userId, 
            PageSize = queryObject.PageSize, 
            Offset = offset
        });
    }
}