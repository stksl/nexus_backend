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
    public async Task<IEnumerable<Comment>> Handle(GetCommentsByPostId request, CancellationToken token) 
    {
        QueryObject queryObject = new QueryObject() 
        {
            PageNumber = request.PageNumber,
        };
        IEnumerable<Comment> comments = await _commentReadRepository.GetCommentsByPostId(request.PostId, queryObject);

        return comments;
    }
}