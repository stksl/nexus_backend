using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;
using Nexus.Application.Extensions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record UpdatePostCommand(int PostId, int UserId, string Content, string Headline, IEnumerable<string> Tags) : ICommand<bool>;

public class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, bool>
{
    private readonly IConfiguration _config;
    private readonly IPostRepository _postRepository;
    private readonly IPostReadRepository _postReadRepository;
    private readonly IPostTagRepository _postTagRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITagReadRepository _tagReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdatePostCommand> _validator;
    public UpdatePostCommandHandler(IConfiguration config,
        IPostRepository postRepository,
        IPostReadRepository postReadRepository,
        IPostTagRepository postTagRepository,
        ITagRepository tagRepository,
        ITagReadRepository tagReadRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdatePostCommand> validator)
    {
        _config = config;
        _postRepository = postRepository;
        _postReadRepository = postReadRepository;
        _postTagRepository = postTagRepository;
        _tagRepository = tagRepository;
        _tagReadRepository = tagReadRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request);

        PostResponse? post = await _postReadRepository.GetPostWithLikesById(request.PostId);

        if (post == null)
            throw new KeyNotFoundException();

        if (post.UserId != request.UserId)
            throw new AuthException("Only owners can update their posts!");

        await AhoCorasickHelper.ReplaceBannedWordsFor(request, _config["BannedWordsPath"]!);

        _mapper.Map(request, (Post)post);

        post.LastModified = DateTime.UtcNow;
        _postRepository.UpdatePost(post);

        foreach (string tag in request.Tags)
        {
            if (await _tagReadRepository.GetTagByName(tag) == null)
                await _tagRepository.AddTag(new Tag() { Name = tag });
        }

        await _unitOfWork.SaveChangesAsync();
        
        _postTagRepository.ClearTags(post);
        await _postTagRepository.AttachTags(post, request.Tags);

        int updated = await _unitOfWork.SaveChangesAsync();

        return updated > 0;
    }
}