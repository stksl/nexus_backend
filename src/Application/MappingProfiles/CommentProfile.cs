using AutoMapper;
using Nexus.Application.Dtos;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class CommentProfile : Profile 
{
    public CommentProfile()
    {
        CreateMap<CreateCommentCommand, Comment>();
    
        CreateMap<UpdateCommentCommand, Comment>();
    }
}