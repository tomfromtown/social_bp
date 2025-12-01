using MediatR;
using SocialMedia.Application.DTOs;

namespace SocialMedia.Application.Queries.GetPosts;

public class GetPostsQuery : IRequest<List<PostDto>>
{
    public int? CurrentUserId { get; set; } // Optional: to determine if current user liked each post
}

