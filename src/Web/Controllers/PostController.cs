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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreatePost([FromBody]CreatePostRequest createPostRequest) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            return Unauthorized();

        var createPostCommand = new CreatePostCommand(
            int.Parse(idClaim.Value), 
            createPostRequest.Content, 
            createPostRequest.Headline,
            createPostRequest.Tags);

        int postId = await _mediator.Send(createPostCommand);
        
        return Ok(postId);
    }
    [HttpPut("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePost([FromBody]UpdatePostRequest updatePostRequest) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            return Unauthorized();

        UpdatePostCommand updatePostCommand = new UpdatePostCommand(
            updatePostRequest.PostId,
            int.Parse(idClaim.Value),
            updatePostRequest.Content,
            updatePostRequest.Headline,
            updatePostRequest.Tags
        );

        bool result = await _mediator.Send(updatePostCommand);
        
        return Ok(result);
    }
    [HttpDelete("delete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeletePost([FromQuery]int postId) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            return Unauthorized();

        DeletePostCommand deletePostCommand = new DeletePostCommand(int.Parse(idClaim.Value), postId);

        bool result = await _mediator.Send(deletePostCommand);
        
        return Ok(result);
    }
    [HttpGet("get")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostById([FromQuery]int postId) 
    {
        var getByIdQuery = new GetPostByIdQuery(postId);
        Post? post = await _mediator.Send(getByIdQuery);
        
        if (post == null)
            return NotFound();
        
        return Ok(post);
    }
    
    [HttpGet("getByUser")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostsByUser([FromQuery]int userId, [FromQuery]int? pageNumber) 
    {
        var getByUserQuery = new GetPostsByUserQuery(userId, pageNumber ?? 1);
        
        IEnumerable<Post> posts = await _mediator.Send(getByUserQuery);
        
        return Ok(posts);
    }
}