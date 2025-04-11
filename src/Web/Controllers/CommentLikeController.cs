using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;

namespace Nexus.WebApi;

[ApiController]
[Route("api/commentLikes")]
public class CommentLikeController : ControllerBase 
{
    private readonly IMediator _mediator;
    public CommentLikeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("like")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddCommentLike([FromQuery]int commentId) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim == null)
            return Unauthorized();

        return Ok(await _mediator.Send(new AddCommentLikeCommand(int.Parse(idClaim.Value), commentId)));
    }

    [HttpDelete("unlike")]    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveCommentLike([FromQuery]int commentId) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim == null)
            return Unauthorized();

        return Ok(await _mediator.Send(new RemoveCommentLikeCommand(int.Parse(idClaim.Value), commentId)));
    }
}