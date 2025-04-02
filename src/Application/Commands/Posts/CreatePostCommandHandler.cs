using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, int>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostTagRepositoryHelper _postTagRepositoryHelper;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePostCommand> _validator;
    public CreatePostCommandHandler(IPostRepository postRepository, 
        IPostTagRepositoryHelper postTagRepository, 
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IValidator<CreatePostCommand> validator)
    {
        _postRepository = postRepository;
        _postTagRepositoryHelper = postTagRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request);

        Post post = _mapper.Map<CreatePostCommand, Post>(request);

        post.DateCreated = DateTime.UtcNow;

        await _postRepository.AddPost(post);
        await _unitOfWork.SaveChangesAsync();

        await _postTagRepositoryHelper.AttachTags(post, request.Tags);

        return post.Id;
    }
}