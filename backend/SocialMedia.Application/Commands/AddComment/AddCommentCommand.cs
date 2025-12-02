using MediatR;
using SocialMedia.Application.DTOs;

namespace SocialMedia.Application.Commands.AddComment;

public class AddCommentCommand : IRequest<CommentDto?>
{
    public int UserId { get; init; }
    public int PostId { get; init; }
    public string Content { get; init; } = string.Empty;
}

