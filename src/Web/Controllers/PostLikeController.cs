using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;

namespace Nexus.WebApi;

[ApiController]
[Route("api/postLikes")]
public class PostLikeController : ControllerBase 
{
    private readonly IMediator _mediator;
    public PostLikeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("like")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddPostLike([FromQuery]int postId) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim == null)
            return Unauthorized();
        
        return Ok(await _mediator.Send(new AddPostLikeCommand(int.Parse(idClaim.Value), postId)));
    }

    [HttpDelete("unline")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemovePostLike([FromQuery]int postId) 
    {
        Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim == null)
            return Unauthorized();

        return Ok(await _mediator.Send(new RemovePostLikeCommand(int.Parse(idClaim.Value), postId)));
    }
}