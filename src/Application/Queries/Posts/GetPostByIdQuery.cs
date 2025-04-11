using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;

namespace Nexus.Application;

public record GetPostByIdQuery(int Id) : IQuery<PostResponse?>;

public class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostResponse?>
{
    private readonly IPostReadRepository _postReadRepository;
    public GetPostByIdQueryHandler(IPostReadRepository postReadRepository)
    {
        _postReadRepository = postReadRepository;
    }
    public async Task<PostResponse?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        PostResponse? post = await _postReadRepository.GetPostWithLikesById(request.Id);

        return post;
    }
}