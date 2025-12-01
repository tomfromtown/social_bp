namespace SocialMedia.Application.DTOs;

public class PostDto
{
    public int Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
    public List<string> LikedBy { get; set; } = new(); // Usernames of users who liked this post
    public bool IsLiked { get; set; } // Whether the current user liked this post
    public List<CommentDto> Comments { get; set; } = new();
}

