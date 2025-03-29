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
    public async Task<Result<IEnumerable<Post>>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Post> posts = await _postReadRepository.GetPostsByUser(request.UserId);

        return Result.Success(posts);
    }
}