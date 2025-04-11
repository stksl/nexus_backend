using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record RemoveCommentLikeCommand(int UserId, int CommentId) : ICommand<bool>;

public class RemoveCommentLikeCommandHandler : ICommandHandler<RemoveCommentLikeCommand, bool> 
{
    private readonly ICommentLikeRepository _commentLikeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveCommentLikeCommandHandler(ICommentLikeRepository commentLikeRepository, IUnitOfWork unitOfWork)
    {
        _commentLikeRepository = commentLikeRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(RemoveCommentLikeCommand request, CancellationToken token) 
    {
        _commentLikeRepository.RemoveCommentLike(request.UserId, request.CommentId);
        int deleted = await _unitOfWork.SaveChangesAsync();

        return deleted == 1;
    }
}