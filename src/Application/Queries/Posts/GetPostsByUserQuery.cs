using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;

namespace Nexus.Application;

public record GetPostsByUserQuery(int UserId, int PageNumber) : IQuery<IEnumerable<PostResponse>>;

public class GetPostsByUserQueryHandler : IQueryHandler<GetPostsByUserQuery, IEnumerable<PostResponse>> 
{
    private readonly IPostReadRepository _postReadRepository;
    public GetPostsByUserQueryHandler(IPostReadRepository postReadRepository)
    {
        _postReadRepository = postReadRepository;
    }
    public async Task<IEnumerable<PostResponse>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
    {
        QueryObject queryObject = new QueryObject() 
        {
            PageNumber = request.PageNumber,
        };
        IEnumerable<PostResponse> posts = await _postReadRepository.GetPostsWithLikesByUser(request.UserId, queryObject);

        return posts;
    }
}