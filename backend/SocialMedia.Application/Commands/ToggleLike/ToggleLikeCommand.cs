using MediatR;

namespace SocialMedia.Application.Commands.ToggleLike;

public class ToggleLikeCommand : IRequest<bool>
{
    public int PostId { get; init; }
    public int UserId { get; init; }
}

