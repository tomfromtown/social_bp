using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Commands.AddComment;
using System.Security.Claims;

namespace SocialMedia.Api.Controllers;

[ApiController]
[Route("api/posts/{postId}/[controller]")]
[Authorize]
public class CommentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddComment(int postId, [FromBody] CreateCommentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized();
        }

        var command = new AddCommentCommand
        {
            UserId = userId,
            PostId = postId,
            Content = request.Content
        };

        var comment = await mediator.Send(command);

        if (comment == null)
        {
            return NotFound(new { message = "Post not found" });
        }

        return CreatedAtAction(nameof(AddComment), new { postId, id = comment.Id }, comment);
    }
}

public class CreateCommentRequest
{
    public string Content { get; set; } = string.Empty;
}
