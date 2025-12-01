using MediatR;
using SocialMedia.Application.DTOs;

namespace SocialMedia.Application.Commands.AddComment;

public class AddCommentCommand : IRequest<CommentDto?>
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; } = string.Empty;
}

