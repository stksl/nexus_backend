using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record AddCommentLikeCommand(int UserId, int CommentId) : ICommand<bool>;

public class AddCommentLikeCommandHandler : ICommandHandler<AddCommentLikeCommand, bool> 
{
    private readonly ICommentLikeRepository _commentLikeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddCommentLikeCommandHandler(ICommentLikeRepository commentLikeRepository, IUnitOfWork unitOfWork)
    {
        _commentLikeRepository = commentLikeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddCommentLikeCommand request, CancellationToken token) 
    {
        await _commentLikeRepository.AddCommentLike(request.UserId, request.CommentId);
        int created = await _unitOfWork.SaveChangesAsync();

        return created == 1;
    }
}