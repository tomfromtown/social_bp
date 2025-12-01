using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.DTOs;

public class CreatePostRequest
{
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
}

