using System.Net.Security;
using AutoMapper;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

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
        Post? post = await _postReadRepository.GetPostById(request.PostId);

        if (post == null)
            throw new KeyNotFoundException();
        
        if (post.UserId != request.UserId)
            throw new AuthException("Only owners can delete their posts!");
        
        _postRepository.RemovePost(post);

        int deleted = await _unitOfWork.SaveChangesAsync();

        return deleted == 1;
    }
}