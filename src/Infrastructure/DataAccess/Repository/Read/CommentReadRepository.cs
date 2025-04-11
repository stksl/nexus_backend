using System.Data;
using Dapper;
using Nexus.Application;
using Nexus.Application.Dtos;

namespace Nexus.Infrastructure.DataAccess;

public class CommentReadRepository : ICommentReadRepository 
{
    private readonly IDbConnection _dbConnection;
    public CommentReadRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CommentResponse?> GetCommentWithLikesById(int id) 
    {
        const string sql = 
            "SELECT \"Id\", c.\"UserId\", \"PostId\", \"Content\", \"DateCreated\", \"LastModified\", \"ParentCommentId\", COUNT(\"CommentId\") AS \"LikeCount\" " +
            "FROM \"Comments\" AS c " +
            "LEFT JOIN \"CommentLikes\" AS cl  " +
            "ON cl.\"CommentId\" = c.\"Id\" " +
            "WHERE \"Id\" = @Id " +
            "GROUP BY \"Id\"" ;

        CommentResponse? comment = await _dbConnection.QueryFirstOrDefaultAsync<CommentResponse>(sql, new {Id = id});

        return comment;
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentsWithLikesByPostId(int postId, QueryObject queryObject) 
    {
        int offset = (queryObject.PageNumber - 1) * queryObject.PageSize;

        const string sql = "SELECT " +
            "\"Id\", " +  
            "c.\"UserId\", " +  
            "\"PostId\", " + 
            "\"Content\", " + 
            "\"DateCreated\", " + 
            "\"LastModified\", " + 
            "\"ParentCommentId\", " + 
            "COUNT(\"CommentId\") AS \"LikeCount\" " +
            "FROM \"Comments\" AS c " +
            "LEFT JOIN \"CommentLikes\" AS cl ON cl.\"CommentId\" = c.\"Id\" " +
            "WHERE c.\"PostId\" = @PostId " +
            "GROUP BY \"Id\" " + 
            "LIMIT @PageSize OFFSET @Offset";

        return await _dbConnection.QueryAsync<CommentResponse>(sql, new 
        {
            PostId = postId, 
            PageSize = queryObject.PageSize, 
            Offset = offset
        });
    }
}