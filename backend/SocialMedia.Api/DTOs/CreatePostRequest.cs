using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.DTOs;

public record CreatePostRequest
{
    [Required]
    [MaxLength(2000)]
    public string Content { get; init; } = string.Empty;
}

