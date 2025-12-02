namespace SocialMedia.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<Comment> Comments { get; set; } = [];
    public List<Like> Likes { get; set; } = [];
}

