using System.Linq.Expressions;
using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;

namespace Nexus.Application;

public record GetCommentsByPostIdQuery(int PostId, int PageNumber, string? SortBy) : IQuery<IEnumerable<CommentResponse>>;

public class GetCommentsByPostIdHandler : IQueryHandler<GetCommentsByPostIdQuery, IEnumerable<CommentResponse>> 
{
    private readonly ICommentReadRepository _commentReadRepository;
    public GetCommentsByPostIdHandler(ICommentReadRepository commentReadRepository)
    {
        _commentReadRepository = commentReadRepository;
    }
    public async Task<IEnumerable<CommentResponse>> Handle(GetCommentsByPostIdQuery request, CancellationToken token) 
    {
        QueryObject<CommentResponse> queryObject = new QueryObject<CommentResponse>() 
        {
            PageNumber = request.PageNumber
        };
        
        if (request.SortBy != null) 
        {
            string[] parts = request.SortBy.Split(' ');
            ParameterExpression parameterExp = Expression.Parameter(typeof(CommentResponse));

            MemberExpression memberExp = Expression.MakeMemberAccess(parameterExp, 
                typeof(CommentResponse).GetProperties().First(prop => prop.Name.ToLower() == parts[0]));
                
            queryObject.SortBy = Expression.Lambda(memberExp, parameterExp);
            queryObject.SortByAscending = parts.Length == 1 || parts[1].ToLower() == "asc";
        }
        IEnumerable<CommentResponse> comments = await _commentReadRepository.GetCommentsWithLikesByPostId(request.PostId, queryObject);
 
        return comments;
    }
}