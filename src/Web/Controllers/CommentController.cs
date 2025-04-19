using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Application.Dtos;
using Nexus.WebApi.Dtos;

namespace Nexus.WebApi;
[ApiController]
[Route("/api/comments")]
public class CommentController : ControllerBase 
{
    private readonly IMediator _mediator;
    public CommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateComment([FromBody]CreateCommentRequest createCommentRequest) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            return Unauthorized();
        
        var createCommentCommand = new CreateCommentCommand(
            createCommentRequest.Content,
            int.Parse(idClaim.Value),
            createCommentRequest.PostId,
            createCommentRequest.ParentCommentId
        );

        int commentId = await _mediator.Send(createCommentCommand);

        return Ok(commentId);
    }
    [HttpPut("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateComment([FromQuery]int commentId, [FromBody]UpdateCommentRequest updateCommentRequest) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            return Unauthorized();
        
        var updateCommentCommand = new UpdateCommentCommand(
            int.Parse(idClaim.Value),
            commentId,
            updateCommentRequest.Content
        );

        bool result = await _mediator.Send(updateCommentCommand);

        return Ok(result);
    }
    [HttpDelete("delete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteComment([FromQuery]int commentId) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            return Unauthorized();
        
        var updateCommentCommand = new DeleteCommentCommand(int.Parse(idClaim.Value), commentId);

        bool result = await _mediator.Send(updateCommentCommand);

        return Ok(result);
    }
    [HttpGet("getByPost")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPostId([FromQuery]int postId, [FromQuery]QueryObjectRequest queryObjectRequest) 
    {
        var getCommentsByPostQuery = new GetCommentsByPostIdQuery(postId, queryObjectRequest.PageNumber ?? 1, queryObjectRequest.SortBy);
        IEnumerable<CommentResponse> comments = await _mediator.Send(getCommentsByPostQuery);
        
        return Ok(comments);
    }
}