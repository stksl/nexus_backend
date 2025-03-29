using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class GetPostsByUserQueryHandler : IQueryHandler<GetPostsByUserQuery, IEnumerable<Post>> 
{
    private readonly IPostReadRepository _postReadRepository;
    public GetPostsByUserQueryHandler(IPostReadRepository postReadRepository)
    {
        _postReadRepository = postReadRepository;
    }
    public async Task<IEnumerable<Post>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
    {
        QueryObject queryObject = new QueryObject() 
        {
            PageNumber = request.PageNumber,
        };
        IEnumerable<Post> posts = await _postReadRepository.GetPostsByUser(request.UserId, queryObject);

        return posts;
    }
}