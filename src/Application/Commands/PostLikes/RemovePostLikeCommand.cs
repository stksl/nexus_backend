using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record RemovePostLikeCommand(int UserId, int PostId) : ICommand<bool>;

public class RemovePostLikeCommandHandler : ICommandHandler<RemovePostLikeCommand, bool> 
{
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemovePostLikeCommandHandler(IPostLikeRepository postLikeRepository, IUnitOfWork unitOfWork)
    {
        _postLikeRepository = postLikeRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(RemovePostLikeCommand request, CancellationToken token) 
    {
        _postLikeRepository.RemovePostLike(request.UserId, request.PostId);
        int deleted = await _unitOfWork.SaveChangesAsync();

        return deleted == 1;
    }
}