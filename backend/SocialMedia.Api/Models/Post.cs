namespace SocialMedia.Api.Models;

public class Post
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<Like> Likes { get; set; } = new();
}

