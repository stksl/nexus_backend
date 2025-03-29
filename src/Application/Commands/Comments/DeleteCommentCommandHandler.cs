using AutoMapper;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand, bool> 
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICommentReadRepository _commentReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteCommentCommandHandler(ICommentRepository commentRepository, ICommentReadRepository commentReadRepository, IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _commentReadRepository = commentReadRepository;
    }
    public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken token) 
    {
        Comment? comment = await _commentReadRepository.GetCommentById(request.CommentId);

        if (comment == null)
            throw new KeyNotFoundException();
        if (comment.UserId != request.UserId)
            throw new AuthException("Only owners can delete their comments!");
        
        _commentRepository.RemoveComment(comment);
        int deleted = await _unitOfWork.SaveChangesAsync();

        return deleted == 1;
    }
}