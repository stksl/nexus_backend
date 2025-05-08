using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;
using Nexus.Application.Extensions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record UpdateCommentCommand(int UserId, int CommentId, string Content) : ICommand<bool>;

public class UpdateCommentCommandHandler : ICommandHandler<UpdateCommentCommand, bool> 
{
    private readonly IConfiguration _config;
    private readonly ICommentRepository _commentRepository;
    private readonly ICommentReadRepository _commentReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateCommentCommand> _validator;
    public UpdateCommentCommandHandler(IConfiguration config,
        ICommentRepository commentRepository, 
        ICommentReadRepository commentReadRepository, 
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateCommentCommand> validator)
    {
        _config = config;
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _commentReadRepository = commentReadRepository;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<bool> Handle(UpdateCommentCommand request, CancellationToken token) 
    {
        await _validator.ValidateAndThrowAsync(request);
        
        CommentResponse? comment = await _commentReadRepository.GetCommentWithLikesById(request.CommentId);

        if (comment == null)
            throw new KeyNotFoundException();
        if (comment.UserId != request.UserId)
            throw new AuthException("Only owners can Update their comments!");
        
        await AhoCorasickHelper.ReplaceBannedWordsFor(request, _config["BannedWordsPath"]!);

        _mapper.Map(request, (Comment)comment);
        comment.LastModified = DateTime.UtcNow;

        _commentRepository.UpdateComment(comment);
        int updated = await _unitOfWork.SaveChangesAsync();

        return updated == 1;
    }
}