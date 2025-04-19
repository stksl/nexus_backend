using System.Data;
using System.Linq.Expressions;
using Dapper;
using Nexus.Application;
using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;

namespace Nexus.Infrastructure.DataAccess;

public class PostReadRepository : IPostReadRepository
{
    private readonly IDbConnection _dbConnection;
    public PostReadRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task<PostResponse?> GetPostWithLikesById(int id)
    {
        const string sql = "SELECT " +
            "    p.\"Id\", " +
            "    p.\"UserId\", " +
            "    p.\"Headline\", " +
            "    p.\"Content\", " +
            "    p.\"DateCreated\", " +
            "    p.\"LastModified\", " +
            "    ARRAY_AGG(DISTINCT t.\"Name\") FILTER (WHERE t.\"Name\" IS NOT NULL) AS \"Tags\", " +
            "    COUNT(DISTINCT pl.\"UserId\") AS \"LikeCount\" " +
            "FROM \"Posts\" p " +
            "LEFT JOIN \"PostLikes\" pl ON pl.\"PostId\" = p.\"Id\" " +
            "LEFT JOIN \"PostTags\" pt ON pt.\"PostId\" = p.\"Id\" " +
            "LEFT JOIN \"Tags\" t ON t.\"Id\" = pt.\"TagId\" " +
            "WHERE p.\"Id\" = @Id " +
            "GROUP BY p.\"Id\" ";

        return _dbConnection.QueryFirstOrDefaultAsync<PostResponse>(sql, new { Id = id });
    }

    public Task<IEnumerable<PostResponse>> GetPostsWithLikesByUser(int userId, QueryObject<PostResponse> queryObject)
    {
        int offset = (queryObject.PageNumber - 1) * queryObject.PageSize;

        string sql = "SELECT " +
            "p.\"Id\", " +
            "p.\"UserId\", " +
            "p.\"Headline\", " +
            "p.\"Content\", " +
            "p.\"DateCreated\", " +
            "p.\"LastModified\", " +
            "ARRAY_AGG(DISTINCT t.\"Name\") FILTER (WHERE t.\"Name\" IS NOT NULL) AS \"Tags\", " +
            "COUNT(DISTINCT pl.\"UserId\") AS \"LikeCount\" " +
            "FROM \"Posts\" p " +
            "LEFT JOIN \"PostLikes\" pl ON pl.\"PostId\" = p.\"Id\" " +
            "LEFT JOIN \"PostTags\" pt ON pt.\"PostId\" = p.\"Id\" " +
            "LEFT JOIN \"Tags\" t ON pt.\"TagId\" = t.\"Id\" " +
            "WHERE p.\"UserId\" = @UserId " +
            "GROUP BY p.\"Id\" ";
            
        if (queryObject.SortBy?.Body is MemberExpression m) 
        {
            sql += $" ORDER BY \"{m.Member.Name}\" " + (queryObject.SortByAscending ? "ASC" : "DESC");
        } 
        sql += " LIMIT @PageSize OFFSET @Offset ";

        return _dbConnection.QueryAsync<PostResponse>(sql, new
        {
            UserId = userId,
            PageSize = queryObject.PageSize,
            Offset = offset
        });
    }
}