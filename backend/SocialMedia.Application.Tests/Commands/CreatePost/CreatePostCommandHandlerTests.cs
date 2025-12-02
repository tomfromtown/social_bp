using FluentAssertions;
using SocialMedia.Application.Commands.CreatePost;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Interfaces;
using SocialMedia.Application.Tests.Helpers;
using SocialMedia.Domain.Entities;
using Xunit;

namespace SocialMedia.Application.Tests.Commands.CreatePost;

public class CreatePostCommandHandlerTests
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesPost()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = "hashedpassword"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var handler = new CreatePostCommandHandler(_context);
        var command = new CreatePostCommand
        {
            UserId = user.Id,
            Content = "Test post content"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        result.Author.Should().Be("testuser");
        result.Content.Should().Be("Test post content");
        result.LikeCount.Should().Be(0);
        result.Comments.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_InvalidUserId_ReturnsNull()
    {
        // Arrange
        var handler = new CreatePostCommandHandler(_context);
        var command = new CreatePostCommand
        {
            UserId = 999, // Non-existent user
            Content = "Test post content"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ValidRequest_SetsCreatedAt()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = "hashedpassword"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var handler = new CreatePostCommandHandler(_context);
        var command = new CreatePostCommand
        {
            UserId = user.Id,
            Content = "Test post content"
        };

        var beforeCreation = DateTime.UtcNow;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        var afterCreation = DateTime.UtcNow;

        // Assert
        result.Should().NotBeNull();
        result!.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        result.CreatedAt.Should().BeOnOrBefore(afterCreation);
    }
}

