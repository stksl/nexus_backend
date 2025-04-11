using AutoMapper;
using Nexus.Application.Dtos;
using Nexus.Domain.Entities;

namespace Nexus.Application;

public class PostProfile : Profile 
{
    public PostProfile()
    {
        CreateMap<CreatePostCommand, Post>();

        CreateMap<UpdatePostCommand, Post>();
    }
}