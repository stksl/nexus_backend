using AutoMapper;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand>
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        Post post = _mapper.Map<CreatePostCommand, Post>(request);

        await _postRepository.AddPost(post);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}