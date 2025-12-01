using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Interfaces;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Application.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDto?>
{
    private readonly IApplicationDbContext _context;

    public AddCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommentDto?> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken);
        if (post == null)
        {
            return null;
        }

        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        if (user == null)
        {
            return null;
        }

        var comment = new Comment
        {
            PostId = request.PostId,
            AuthorId = request.UserId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with author
        var createdComment = await _context.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == comment.Id, cancellationToken);

        if (createdComment == null)
        {
            return null;
        }

        return new CommentDto
        {
            Id = createdComment.Id,
            Author = createdComment.Author.Username,
            Content = createdComment.Content,
            CreatedAt = createdComment.CreatedAt
        };
    }
}

