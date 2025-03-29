using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class GetCommentsByPostIdHandler : IQueryHandler<GetCommentsByPostId, IEnumerable<Comment>> 
{
    private readonly ICommentReadRepository _commentReadRepository;
    public GetCommentsByPostIdHandler(ICommentReadRepository commentReadRepository)
    {
        _commentReadRepository = commentReadRepository;
    }
    public async Task<Result<IEnumerable<Comment>>> Handle(GetCommentsByPostId request, CancellationToken token) 
    {
        IEnumerable<Comment> comments = await _commentReadRepository.GetCommentsByPostId(request.PostId);

        return Result.Success(comments);
    }
}