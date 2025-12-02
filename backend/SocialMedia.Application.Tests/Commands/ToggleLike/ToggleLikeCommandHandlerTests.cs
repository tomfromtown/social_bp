using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Commands.ToggleLike;
using SocialMedia.Application.Interfaces;
using SocialMedia.Application.Tests.Helpers;
using SocialMedia.Domain.Entities;
using Xunit;

namespace SocialMedia.Application.Tests.Commands.ToggleLike;

public class ToggleLikeCommandHandlerTests
{
    private readonly IApplicationDbContext _context;

    public ToggleLikeCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
    }

    [Fact]
    public async Task Handle_NoExistingLike_AddsLikeAndReturnsTrue()
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

        var handler = new ToggleLikeCommandHandler(_context);
        var command = new ToggleLikeCommand
        {
            PostId = post.Id,
            UserId = user.Id
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == post.Id && l.UserId == user.Id);
        like.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ExistingLike_RemovesLikeAndReturnsFalse()
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

        var like = new Like
        {
            PostId = post.Id,
            UserId = user.Id
        };

        _context.Likes.Add(like);
        await _context.SaveChangesAsync();

        var handler = new ToggleLikeCommandHandler(_context);
        var command = new ToggleLikeCommand
        {
            PostId = post.Id,
            UserId = user.Id
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        var existingLike = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == post.Id && l.UserId == user.Id);
        existingLike.Should().BeNull();
    }

    [Fact]
    public async Task Handle_InvalidPostId_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = "hashedpassword"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var handler = new ToggleLikeCommandHandler(_context);
        var command = new ToggleLikeCommand
        {
            PostId = 999, // Non-existent post
            UserId = user.Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidUserId_ThrowsInvalidOperationException()
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

        var handler = new ToggleLikeCommandHandler(_context);
        var command = new ToggleLikeCommand
        {
            PostId = post.Id,
            UserId = 999 // Non-existent user
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(command, CancellationToken.None));
    }
}

