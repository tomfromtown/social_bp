using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Interfaces;
using SocialMedia.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Application.Commands.Login;

public class LoginCommandHandler(IApplicationDbContext context, IConfiguration configuration)
    : IRequestHandler<LoginCommand, LoginResponse?>
{
    public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var token = GenerateJwtToken(user);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
        var issuer = jwtSettings["Issuer"] ?? "SocialMediaApi";
        var audience = jwtSettings["Audience"] ?? "SocialMediaUsers";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

