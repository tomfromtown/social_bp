using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Interfaces;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Infrastructure.Data;

public static class SeedData
{
    public static void Initialize(IApplicationDbContext context)
    {
        var dbContext = context as DbContext;
        if (dbContext == null) return;

        // Check if database has been seeded
        try
        {
            if (dbContext.Set<User>().Any())
            {
                return; // Database has been seeded
            }
        }
        catch
        {
            // Tables don't exist yet, migrations will create them
            // This is expected on first run
        }

        // Create test user
        var testUser = new User
        {
            Username = "test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("test")
        };

        dbContext.Set<User>().Add(testUser);
        dbContext.SaveChanges(); // Save to get the ID

        // Create sample users
        var alice = new User
        {   
            Username = "Alice Johnson",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };

        var bob = new User
        {
            Username = "Bob Smith",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };

        var charlie = new User
        {
            Username = "Charlie Brown",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };

        dbContext.Set<User>().AddRange(alice, bob, charlie);
        dbContext.SaveChanges(); // Save to get IDs

        // Get user IDs after saving
        var testUserId = testUser.Id;
        var aliceId = alice.Id;
        var bobId = bob.Id;
        var charlieId = charlie.Id;

        // Create sample posts
        var posts = new List<Post>
        {
            new Post
            {
                AuthorId = aliceId,
                Content = "Just finished reading an amazing book! 📚 What are you all reading?",
                CreatedAt = DateTime.UtcNow.AddHours(-2)
            },
            new Post
            {
                AuthorId = bobId,
                Content = "Beautiful sunset today! 🌅 Nature never fails to amaze me.",
                CreatedAt = DateTime.UtcNow.AddHours(-5)
            },
            new Post
            {
                AuthorId = charlieId,
                Content = "Just learned a new programming concept today. Always keep learning! 💻",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        dbContext.Set<Post>().AddRange(posts);
        dbContext.SaveChanges(); // Save to get post IDs

        var post1Id = posts[0].Id;
        var post2Id = posts[1].Id;
        var post3Id = posts[2].Id;

        // Create sample comments
        var comments = new List<Comment>
        {
            new Comment
            {
                PostId = post1Id,
                AuthorId = bobId,
                Content = "I just finished \"The Great Gatsby\"!",
                CreatedAt = DateTime.UtcNow.AddHours(-1)
            },
            new Comment
            {
                PostId = post1Id,
                AuthorId = charlieId,
                Content = "Currently reading \"1984\" - highly recommend!",
                CreatedAt = DateTime.UtcNow.AddMinutes(-30)
            },
            new Comment
            {
                PostId = post2Id,
                AuthorId = aliceId,
                Content = "Stunning! Where was this taken?",
                CreatedAt = DateTime.UtcNow.AddHours(-4)
            }
        };

        dbContext.Set<Comment>().AddRange(comments);
        dbContext.SaveChanges();

        // Create sample likes
        var likes = new List<Like>
        {
            new Like { PostId = post1Id, UserId = aliceId },
            new Like { PostId = post1Id, UserId = bobId },
            new Like { PostId = post1Id, UserId = charlieId },
            new Like { PostId = post2Id, UserId = aliceId },
            new Like { PostId = post2Id, UserId = charlieId },
            new Like { PostId = post3Id, UserId = aliceId },
            new Like { PostId = post3Id, UserId = bobId }
        };

        dbContext.Set<Like>().AddRange(likes);
        dbContext.SaveChanges();
    }
}

