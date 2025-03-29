using System.Data;
using System.Linq.Expressions;
using Dapper;
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

    public Task<IEnumerable<Post>> GetPostsByUser(int userId)
    {
        const string sql = "SELECT * FROM \"Posts\" WHERE \"UserId\" = @UserId";

        return _dbConnection.QueryAsync<Post>(sql, new {UserId = userId});
    }
}