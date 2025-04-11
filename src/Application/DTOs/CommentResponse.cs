
using Nexus.Domain.Entities;

namespace Nexus.Application.Dtos;

public class CommentResponse : Comment
{
    public int LikesCount {get; set;}
}