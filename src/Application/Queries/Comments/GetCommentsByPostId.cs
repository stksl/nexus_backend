using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;

namespace Nexus.Application;

public record GetCommentsByPostId(int PostId, int PageNumber) : IQuery<IEnumerable<CommentResponse>>;

public class GetCommentsByPostIdHandler : IQueryHandler<GetCommentsByPostId, IEnumerable<CommentResponse>> 
{
    private readonly ICommentReadRepository _commentReadRepository;
    public GetCommentsByPostIdHandler(ICommentReadRepository commentReadRepository)
    {
        _commentReadRepository = commentReadRepository;
    }
    public async Task<IEnumerable<CommentResponse>> Handle(GetCommentsByPostId request, CancellationToken token) 
    {
        QueryObject queryObject = new QueryObject() 
        {
            PageNumber = request.PageNumber,
        };
        IEnumerable<CommentResponse> comments = await _commentReadRepository.GetCommentsWithLikesByPostId(request.PostId, queryObject);
 
        return comments;
    }
}