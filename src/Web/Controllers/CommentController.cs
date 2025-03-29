using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Domain.Entities;

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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        Result<int> result = await _mediator.Send(createCommentCommand);

        if (!result.Succeed)
            return BadRequest();
        
        return Ok(result.ResultValue);
    }
    [HttpGet("getByPost")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByPostId([FromQuery]int postId) 
    {
        // todo: add paging
        var getCommentsByPostQuery = new GetCommentsByPostId(postId);
        Result<IEnumerable<Comment>> result = await _mediator.Send(getCommentsByPostQuery);

        if (!result.Succeed)
            return BadRequest();
        
        return Ok(result.ResultValue);
    }
}