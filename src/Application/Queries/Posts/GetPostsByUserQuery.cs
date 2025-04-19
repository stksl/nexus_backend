using System.Linq.Expressions;
using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;

namespace Nexus.Application;

public record GetPostsByUserQuery(int UserId, int PageNumber, string? SortBy) : IQuery<IEnumerable<PostResponse>>;

public class GetPostsByUserQueryHandler : IQueryHandler<GetPostsByUserQuery, IEnumerable<PostResponse>> 
{
    private readonly IPostReadRepository _postReadRepository;
    public GetPostsByUserQueryHandler(IPostReadRepository postReadRepository)
    {
        _postReadRepository = postReadRepository;
    }
    public async Task<IEnumerable<PostResponse>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
    {
        QueryObject<PostResponse> queryObject = new QueryObject<PostResponse>() 
        {
            PageNumber = request.PageNumber,
        };

        if (request.SortBy != null) 
        {
            string[] parts = request.SortBy.Split(' ');
            ParameterExpression parameterExp = Expression.Parameter(typeof(PostResponse));

            MemberExpression memberExp = Expression.MakeMemberAccess(parameterExp, 
                typeof(PostResponse).GetProperties().First(prop => prop.Name.ToLower() == parts[0]));
                
            queryObject.SortBy = Expression.Lambda(memberExp, parameterExp);
            queryObject.SortByAscending = parts.Length == 1 || parts[1].ToLower() == "asc";
        }

        IEnumerable<PostResponse> posts = await _postReadRepository.GetPostsWithLikesByUser(request.UserId, queryObject);

        return posts;
    }
}