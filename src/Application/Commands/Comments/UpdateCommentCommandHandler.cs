using AutoMapper;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class UpdateCommentCommandHandler : ICommandHandler<UpdateCommentCommand, bool> 
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICommentReadRepository _commentReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateCommentCommandHandler(ICommentRepository commentRepository, 
        ICommentReadRepository commentReadRepository, 
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _commentReadRepository = commentReadRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateCommentCommand request, CancellationToken token) 
    {
        Comment? comment = await _commentReadRepository.GetCommentById(request.CommentId);

        if (comment == null)
            throw new KeyNotFoundException();
        if (comment.UserId != request.UserId)
            throw new AuthException("Only owners can Update their comments!");
        
        _mapper.Map(request, comment);
        comment.LastModified = DateTime.UtcNow;

        _commentRepository.UpdateComment(comment);
        int updated = await _unitOfWork.SaveChangesAsync();

        return updated == 1;
    }
}