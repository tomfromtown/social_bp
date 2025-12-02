using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.DTOs;

public record CreateCommentRequest
{
    [Required]
    [MaxLength(500)]
    public string Content { get; init; } = string.Empty;
}

