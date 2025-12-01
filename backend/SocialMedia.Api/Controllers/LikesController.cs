using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Commands.ToggleLike;
using System.Security.Claims;

namespace SocialMedia.Api.Controllers;

[ApiController]
[Route("api/posts/{postId}/likes")]
[Authorize]
public class LikesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LikesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> ToggleLike(int postId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized();
        }

        var command = new ToggleLikeCommand
        {
            PostId = postId,
            UserId = userId
        };

        try
        {
            var isLiked = await _mediator.Send(command);
            return Ok(new { isLiked, message = isLiked ? "Post liked" : "Post unliked" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

