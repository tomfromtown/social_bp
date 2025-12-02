namespace SocialMedia.Api.DTOs;

public record PostDto
{
    public int Id { get; init; }
    public string Author { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public int LikeCount { get; init; }
    public List<CommentDto> Comments { get; init; } = new();
}

