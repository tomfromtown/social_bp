using MediatR;
using SocialMedia.Application.DTOs;

namespace SocialMedia.Application.Commands.Login;

public class LoginCommand : IRequest<LoginResponse?>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

