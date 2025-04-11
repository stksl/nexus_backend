using AutoMapper;
using FluentValidation;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public record CreateCommentCommand(string Content, int UserId, int PostId, int? ParentCommentId) : ICommand<int>;

public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, int> 
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private IValidator<CreateCommentCommand> _validator;
    public CreateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateCommentCommand> validator)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<int> Handle(CreateCommentCommand request, CancellationToken token) 
    {
        await _validator.ValidateAndThrowAsync(request);
        
        Comment comment = _mapper.Map<Comment>(request);

        comment.DateCreated = DateTime.UtcNow;

        await _commentRepository.AddComment(comment);
        await _unitOfWork.SaveChangesAsync();

        return comment.Id;
    }
}