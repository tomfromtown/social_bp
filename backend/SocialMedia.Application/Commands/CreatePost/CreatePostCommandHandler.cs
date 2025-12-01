using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Interfaces;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Application.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto?>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PostDto?> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        if (user == null)
        {
            return null;
        }

        var post = new Post
        {
            AuthorId = request.UserId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with includes
        var createdPost = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == post.Id, cancellationToken);

        if (createdPost == null)
        {
            return null;
        }

        return new PostDto
        {
            Id = createdPost.Id,
            Author = createdPost.Author.Username,
            Content = createdPost.Content,
            CreatedAt = createdPost.CreatedAt,
            LikeCount = createdPost.Likes.Count,
            Comments = createdPost.Comments
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Author = c.Author.Username,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                })
                .ToList()
        };
    }
}

