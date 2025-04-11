using AutoMapper;
using FluentValidation;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record CreatePostCommand(int UserId, string Content, string Headline, IEnumerable<string> Tags) : ICommand<int>;

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, int>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostTagRepository _postTagRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITagReadRepository _tagReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePostCommand> _validator;
    public CreatePostCommandHandler(IPostRepository postRepository, 
        IPostTagRepository postTagRepository,
        ITagRepository tagRepository, 
        ITagReadRepository tagReadRepository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IValidator<CreatePostCommand> validator)
    {
        _postRepository = postRepository;
        _postTagRepository = postTagRepository;
        _tagRepository = tagRepository;
        _tagReadRepository = tagReadRepository;
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

        foreach(string tag in request.Tags) 
        {
            if (await _tagReadRepository.GetTagByName(tag) == null)
                await _tagRepository.AddTag(new Tag() {Name = tag});
        }

        await _unitOfWork.SaveChangesAsync();

        await _postTagRepository.AttachTags(post, request.Tags);

        await _unitOfWork.SaveChangesAsync();

        return post.Id;
    }
}