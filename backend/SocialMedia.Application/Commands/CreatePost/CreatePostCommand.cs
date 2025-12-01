using MediatR;
using SocialMedia.Application.DTOs;

namespace SocialMedia.Application.Commands.CreatePost;

public class CreatePostCommand : IRequest<PostDto?>
{
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
}

