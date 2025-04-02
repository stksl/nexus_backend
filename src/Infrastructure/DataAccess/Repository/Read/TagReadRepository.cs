using System.Data;
using Dapper;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class TagReadRepository : ITagReadRepository 
{
    private IDbConnection _dbConnection;
    public TagReadRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task<Tag?> GetTagByName(string name) 
    {
        const string sql = "SELECT * FROM \"Tags\" WHERE \"Name\" = @Name";
        return _dbConnection.QueryFirstOrDefaultAsync<Tag>(sql, new {Name = name});
    }
}