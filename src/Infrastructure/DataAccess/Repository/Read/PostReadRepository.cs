using System.Data;
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
    public async Task<Post?> GetPostById(int id)
    {
        const string sql = "SELECT * FROM \"Posts\" WHERE \"Id\" = @Id";

        Post? post = await _dbConnection.QueryFirstOrDefaultAsync<Post>(sql, new {Id = id});
        
        return post;
    }
}