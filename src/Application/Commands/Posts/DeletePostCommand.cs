using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;

namespace Nexus.Application;

public record DeletePostCommand(int UserId, int PostId) : ICommand<bool>;

public class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, bool>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostReadRepository _postReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeletePostCommandHandler(IPostRepository postRepository, IPostReadRepository postReadRepository, IUnitOfWork unitOfWork)
    {
        _postRepository = postRepository;
        _postReadRepository = postReadRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        PostResponse? post = await _postReadRepository.GetPostWithLikesById(request.PostId);

        if (post == null)
            throw new KeyNotFoundException();
        
        if (post.UserId != request.UserId)
            throw new AuthException("Only owners can delete their posts!");
        
        _postRepository.RemovePost(post);

        int deleted = await _unitOfWork.SaveChangesAsync();

        return deleted == 1;

        throw new NotImplementedException();
    }
}