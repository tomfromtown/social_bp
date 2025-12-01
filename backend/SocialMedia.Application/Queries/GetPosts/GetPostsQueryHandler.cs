using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Interfaces;

namespace SocialMedia.Application.Queries.GetPosts;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPostsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
            .Include(p => p.Likes)
                .ThenInclude(l => l.User)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            Author = p.Author.Username,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            LikeCount = p.Likes.Count,
            LikedBy = p.Likes
                .Select(l => l.User.Username)
                .ToList(),
            IsLiked = request.CurrentUserId.HasValue && 
                      p.Likes.Any(l => l.UserId == request.CurrentUserId.Value),
            Comments = p.Comments
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Author = c.Author.Username,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                })
                .ToList()
        }).ToList();
    }
}

