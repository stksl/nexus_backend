using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery>
{
    private readonly IPostReadRepository _postReadRepository;
    public GetPostByIdQueryHandler(IPostReadRepository postReadRepository)
    {
        _postReadRepository = postReadRepository;
    }
    public async Task<Result> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        Post? post = await _postReadRepository.GetPostById(request.Id);

        return post == null ? Result.Failed() : Result.Success(post);
    }
}