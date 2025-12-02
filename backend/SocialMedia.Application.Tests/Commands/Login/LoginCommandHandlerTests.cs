using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SocialMedia.Application.Commands.Login;
using SocialMedia.Application.Interfaces;
using SocialMedia.Application.Tests.Helpers;
using SocialMedia.Domain.Entities;
using Xunit;

namespace SocialMedia.Application.Tests.Commands.Login;

public class LoginCommandHandlerTests
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public LoginCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
        _configuration = CreateTestConfiguration();
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var password = "testpassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Username = "testuser",
            PasswordHash = hashedPassword
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var handler = new LoginCommandHandler(_context, _configuration);
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = password
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task Handle_InvalidUsername_ReturnsNull()
    {
        // Arrange
        var handler = new LoginCommandHandler(_context, _configuration);
        var command = new LoginCommand
        {
            Username = "nonexistent",
            Password = "password"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var password = "correctpassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Username = "testuser",
            PasswordHash = hashedPassword
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var handler = new LoginCommandHandler(_context, _configuration);
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ValidCredentials_GeneratesValidJwtToken()
    {
        // Arrange
        var password = "testpassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Username = "testuser",
            PasswordHash = hashedPassword
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var handler = new LoginCommandHandler(_context, _configuration);
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = password
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        
        // Verify token can be parsed
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var canRead = tokenHandler.CanReadToken(result.Token);
        canRead.Should().BeTrue();
    }

    private IConfiguration CreateTestConfiguration()
    {
        var configuration = new Dictionary<string, string?>
        {
            { "JwtSettings:SecretKey", "TestSecretKeyThatIsAtLeast32CharactersLong!" },
            { "JwtSettings:Issuer", "TestIssuer" },
            { "JwtSettings:Audience", "TestAudience" },
            { "JwtSettings:ExpirationInMinutes", "60" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(configuration)
            .Build();
    }
}

