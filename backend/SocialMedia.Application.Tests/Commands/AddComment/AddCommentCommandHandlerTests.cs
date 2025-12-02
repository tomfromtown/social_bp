using FluentAssertions;
using SocialMedia.Application.Commands.AddComment;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Interfaces;
using SocialMedia.Application.Tests.Helpers;
using SocialMedia.Domain.Entities;
using Xunit;

namespace SocialMedia.Application.Tests.Commands.AddComment;

public class AddCommentCommandHandlerTests
{
    private readonly IApplicationDbContext _context;

    public AddCommentCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesComment()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = "hashedpassword"
        };
        var post = new Post
        {
            AuthorId = user.Id,
            Content = "Test post",
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var handler = new AddCommentCommandHandler(_context);
        var command = new AddCommentCommand
        {
            PostId = post.Id,
            UserId = user.Id,
            Content = "Test comment"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        result.Author.Should().Be("testuser");
        result.Content.Should().Be("Test comment");
    }

    [Fact]
    public async Task Handle_InvalidPostId_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = "hashedpassword"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var handler = new AddCommentCommandHandler(_context);
        var command = new AddCommentCommand
        {
            PostId = 999, // Non-existent post
            UserId = user.Id,
            Content = "Test comment"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_InvalidUserId_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = "hashedpassword"
        };
        var post = new Post
        {
            AuthorId = user.Id,
            Content = "Test post",
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var handler = new AddCommentCommandHandler(_context);
        var command = new AddCommentCommand
        {
            PostId = post.Id,
            UserId = 999, // Non-existent user
            Content = "Test comment"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}

