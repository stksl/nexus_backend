using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Abstractions;
using Nexus.Application.Extensions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record CreateCommentCommand(string Content, int UserId, int PostId, int? ParentCommentId = null) : ICommand<int>;

public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, int>
{
    private readonly IConfiguration _config;
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private IValidator<CreateCommentCommand> _validator;
    public CreateCommentCommandHandler(IConfiguration config,
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateCommentCommand> validator)
    {
        _config = config;
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<int> Handle(CreateCommentCommand request, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(request);

        await AhoCorasickHelper.ReplaceBannedWordsFor(request, _config["BannedWordsPath"]!);

        Comment comment = _mapper.Map<Comment>(request);

        comment.DateCreated = DateTime.UtcNow;

        await _commentRepository.AddComment(comment);
        await _unitOfWork.SaveChangesAsync();

        return comment.Id;
    }
}