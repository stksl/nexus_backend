using Nexus.Application.Abstractions;

namespace Nexus.Application;

public record AddPostLikeCommand(int UserId, int PostId) : ICommand<bool>;

public class AddPostLikeCommandHandler : ICommandHandler<AddPostLikeCommand, bool> 
{
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddPostLikeCommandHandler(IPostLikeRepository postLikeRepository, IUnitOfWork unitOfWork)
    {
        _postLikeRepository = postLikeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddPostLikeCommand request, CancellationToken token) 
    {
        await _postLikeRepository.AddPostLike(request.UserId, request.PostId);
        int created = await _unitOfWork.SaveChangesAsync();

        return created == 1;
    }
}