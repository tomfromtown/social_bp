namespace SocialMedia.Api.DTOs;

public record CommentDto
{
    public int Id { get; init; }
    public string Author { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

