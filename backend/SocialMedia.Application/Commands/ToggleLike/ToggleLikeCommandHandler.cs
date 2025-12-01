using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Interfaces;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Application.Commands.ToggleLike;

public class ToggleLikeCommandHandler : IRequestHandler<ToggleLikeCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ToggleLikeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        // Check if post exists
        var postExists = await _context.Posts.AnyAsync(p => p.Id == request.PostId, cancellationToken);
        if (!postExists)
        {
            throw new InvalidOperationException("Post not found");
        }

        // Check if user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found");
        }

        // Check if like already exists
        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.PostId == request.PostId && l.UserId == request.UserId, cancellationToken);

        if (existingLike != null)
        {
            // Unlike: remove the like
            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync(cancellationToken);
            return false; // Return false to indicate unliked
        }
        else
        {
            // Like: add the like
            var like = new Like
            {
                PostId = request.PostId,
                UserId = request.UserId
            };
            _context.Likes.Add(like);
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Return true to indicate liked
        }
    }
}

