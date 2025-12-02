using FluentAssertions;
using SocialMedia.Application.Interfaces;
using SocialMedia.Application.Queries.GetPosts;
using SocialMedia.Application.Tests.Helpers;
using SocialMedia.Domain.Entities;
using Xunit;

namespace SocialMedia.Application.Tests.Queries.GetPosts;

public class GetPostsQueryHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetPostsQueryHandlerTests()
    {
        _context = TestDbContextFactory.Create();
    }

    [Fact]
    public async Task Handle_NoPosts_ReturnsEmptyList()
    {
        // Arrange
        var handler = new GetPostsQueryHandler(_context);
        var query = new GetPostsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithPosts_ReturnsAllPosts()
    {
        // Arrange
        var user1 = new User { Username = "user1", PasswordHash = "hash1" };
        var user2 = new User { Username = "user2", PasswordHash = "hash2" };
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        var post1 = new Post { AuthorId = user1.Id, Content = "Post 1", CreatedAt = DateTime.UtcNow.AddHours(-2) };
        var post2 = new Post { AuthorId = user2.Id, Content = "Post 2", CreatedAt = DateTime.UtcNow.AddHours(-1) };
        _context.Posts.AddRange(post1, post2);
        await _context.SaveChangesAsync();

        var handler = new GetPostsQueryHandler(_context);
        var query = new GetPostsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeInDescendingOrder(p => p.CreatedAt);
    }

    [Fact]
    public async Task Handle_WithPostsAndComments_ReturnsPostsWithComments()
    {
        // Arrange
        var user = new User { Username = "user1", PasswordHash = "hash1" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var post = new Post { AuthorId = user.Id, Content = "Post 1", CreatedAt = DateTime.UtcNow };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var comment1 = new Comment { PostId = post.Id, AuthorId = user.Id, Content = "Comment 1", CreatedAt = DateTime.UtcNow.AddMinutes(-10) };
        var comment2 = new Comment { PostId = post.Id, AuthorId = user.Id, Content = "Comment 2", CreatedAt = DateTime.UtcNow.AddMinutes(-5) };
        _context.Comments.AddRange(comment1, comment2);
        await _context.SaveChangesAsync();

        var handler = new GetPostsQueryHandler(_context);
        var query = new GetPostsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Comments.Should().HaveCount(2);
        result[0].Comments.Should().BeInAscendingOrder(c => c.CreatedAt);
    }

    [Fact]
    public async Task Handle_WithLikes_ReturnsPostsWithLikeCount()
    {
        // Arrange
        var user1 = new User { Username = "user1", PasswordHash = "hash1" };
        var user2 = new User { Username = "user2", PasswordHash = "hash2" };
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        var post = new Post { AuthorId = user1.Id, Content = "Post 1", CreatedAt = DateTime.UtcNow };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var like1 = new Like { PostId = post.Id, UserId = user1.Id };
        var like2 = new Like { PostId = post.Id, UserId = user2.Id };
        _context.Likes.AddRange(like1, like2);
        await _context.SaveChangesAsync();

        var handler = new GetPostsQueryHandler(_context);
        var query = new GetPostsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].LikeCount.Should().Be(2);
        result[0].LikedBy.Should().HaveCount(2);
        result[0].LikedBy.Should().Contain("user1");
        result[0].LikedBy.Should().Contain("user2");
    }

    [Fact]
    public async Task Handle_WithCurrentUserId_SetsIsLikedCorrectly()
    {
        // Arrange
        var user1 = new User { Username = "user1", PasswordHash = "hash1" };
        var user2 = new User { Username = "user2", PasswordHash = "hash2" };
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        var post = new Post { AuthorId = user1.Id, Content = "Post 1", CreatedAt = DateTime.UtcNow };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var like = new Like { PostId = post.Id, UserId = user1.Id };
        _context.Likes.Add(like);
        await _context.SaveChangesAsync();

        var handler = new GetPostsQueryHandler(_context);
        var query = new GetPostsQuery { CurrentUserId = user1.Id };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].IsLiked.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithoutCurrentUserId_SetsIsLikedToFalse()
    {
        // Arrange
        var user = new User { Username = "user1", PasswordHash = "hash1" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var post = new Post { AuthorId = user.Id, Content = "Post 1", CreatedAt = DateTime.UtcNow };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var handler = new GetPostsQueryHandler(_context);
        var query = new GetPostsQuery(); // No CurrentUserId

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].IsLiked.Should().BeFalse();
    }
}

