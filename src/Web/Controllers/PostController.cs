using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Domain.Entities;
using Nexus.WebApi.Dtos;

namespace Nexus.WebApi;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase 
{
    private readonly IMediator _mediator;
    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePost([FromBody]CreatePostRequest createPostRequest) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            return Unauthorized();

        CreatePostCommand createPostCommand = new CreatePostCommand(
            idClaim.Value, 
            createPostRequest.Content, 
            createPostRequest.Headline);

        Result<int> result = await _mediator.Send(createPostCommand);
        if (!result.Succeed)
            return BadRequest();
        
        return Ok(result.ResultValue);
    }
    [HttpGet("get")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostById([FromQuery]int postId) 
    {
        GetPostByIdQuery getByIdQuery = new GetPostByIdQuery(postId);
        Result<Post> post = await _mediator.Send(getByIdQuery);

        if (!post.Succeed)
            return BadRequest();
        
        if (post.ResultValue == null)
            return NotFound();
        
        return Ok(post.ResultValue);
    }
    
    [HttpGet("getByUser")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostsByUser([FromQuery]int userId) 
    {
        GetPostsByUserQuery getByUserQuery = new GetPostsByUserQuery(userId);
        
        // todo: add paging
        Result<IEnumerable<Post>> posts = await _mediator.Send(getByUserQuery);

        if (!posts.Succeed)
            return BadRequest();
        
        if (posts.ResultValue == null)
            return NotFound();
        
        return Ok(posts.ResultValue);
    }
}