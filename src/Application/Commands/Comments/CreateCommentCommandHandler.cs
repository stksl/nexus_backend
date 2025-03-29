using AutoMapper;
using Nexus.Application.Abstractions;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, int> 
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateCommentCommand request, CancellationToken token) 
    {
        Comment comment = _mapper.Map<Comment>(request);

        comment.DateCreated = DateTime.UtcNow;

        await _commentRepository.AddComment(comment);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(comment.Id);
    }
}