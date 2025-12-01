using MediatR;
using SocialMedia.Application.DTOs;

namespace SocialMedia.Application.Commands.Login;

public class LoginCommand : IRequest<LoginResponse?>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

