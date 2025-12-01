using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Commands.CreatePost;
using SocialMedia.Application.Queries.GetPosts;
using System.Security.Claims;

namespace SocialMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var query = new GetPostsQuery();
        var posts = await _mediator.Send(query);
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
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

        var command = new CreatePostCommand
        {
            UserId = userId,
            Content = request.Content
        };

        var post = await _mediator.Send(command);

        if (post == null)
        {
            return BadRequest(new { message = "Failed to create post" });
        }

        return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
    }
}

public class CreatePostRequest
{
    public string Content { get; set; } = string.Empty;
}
