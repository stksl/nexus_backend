using AutoMapper;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, bool>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostReadRepository _postReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdatePostCommandHandler(IPostRepository postRepository, IPostReadRepository postReadRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _postRepository = postRepository;
        _postReadRepository = postReadRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        Post? post = await _postReadRepository.GetPostById(request.PostId);

        if (post == null)
            throw new KeyNotFoundException();
        
        if (post.UserId != request.UserId)
            throw new AuthException("Only owners can update their comments!");

        _mapper.Map(request, post);

        post.LastModified = DateTime.UtcNow;
        _postRepository.UpdatePost(post);

        int updated = await _unitOfWork.SaveChangesAsync();

        return updated == 1;
    }
}