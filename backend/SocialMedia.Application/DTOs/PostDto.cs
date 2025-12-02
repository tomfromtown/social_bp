namespace SocialMedia.Application.DTOs;

public record PostDto
{
    public int Id { get; init; }
    public string Author { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public int LikeCount { get; init; }
    public List<string> LikedBy { get; init; } = []; // Usernames of users who liked this post
    public bool IsLiked { get; init; } // Whether the current user liked this post
    public List<CommentDto> Comments { get; init; } = new();
}

