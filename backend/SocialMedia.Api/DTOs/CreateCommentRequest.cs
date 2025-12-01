using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.DTOs;

public class CreateCommentRequest
{
    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = string.Empty;
}

