// using Microsoft.EntityFrameworkCore;
// using SocialMedia.Api.Models;
//
// namespace SocialMedia.Api.Data;
//
// public class ApplicationDbContext : DbContext
// {
//     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//         : base(options)
//     {
//     }
//
//     public DbSet<User> Users { get; set; }
//     public DbSet<Post> Posts { get; set; }
//     public DbSet<Comment> Comments { get; set; }
//     public DbSet<Like> Likes { get; set; }
//
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);
//
//         // User configuration
//         modelBuilder.Entity<User>(entity =>
//         {
//             entity.HasKey(e => e.Id);
//             entity.Property(e => e.Id).UseIdentityColumn();
//             entity.HasIndex(e => e.Username).IsUnique();
//             entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
//             entity.Property(e => e.PasswordHash).IsRequired();
//         });
//
//         // Post configuration
//         modelBuilder.Entity<Post>(entity =>
//         {
//             entity.HasKey(e => e.Id);
//             entity.Property(e => e.Id).UseIdentityColumn();
//             entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
//             entity.Property(e => e.CreatedAt).IsRequired();
//             entity.HasOne(e => e.Author)
//                   .WithMany()
//                   .HasForeignKey(e => e.AuthorId)
//                   .OnDelete(DeleteBehavior.Restrict);
//         });
//
//         // Comment configuration
//         modelBuilder.Entity<Comment>(entity =>
//         {
//             entity.HasKey(e => e.Id);
//             entity.Property(e => e.Id).UseIdentityColumn();
//             entity.Property(e => e.Content).IsRequired().HasMaxLength(500);
//             entity.Property(e => e.CreatedAt).IsRequired();
//             entity.HasOne(e => e.Post)
//                   .WithMany(p => p.Comments)
//                   .HasForeignKey(e => e.PostId)
//                   .OnDelete(DeleteBehavior.Cascade);
//             entity.HasOne(e => e.Author)
//                   .WithMany()
//                   .HasForeignKey(e => e.AuthorId)
//                   .OnDelete(DeleteBehavior.Restrict);
//         });
//
//         // Like configuration
//         modelBuilder.Entity<Like>(entity =>
//         {
//             entity.HasKey(e => e.Id);
//             entity.Property(e => e.Id).UseIdentityColumn();
//             entity.HasIndex(e => new { e.PostId, e.UserId }).IsUnique();
//             entity.HasOne(e => e.Post)
//                   .WithMany(p => p.Likes)
//                   .HasForeignKey(e => e.PostId)
//                   .OnDelete(DeleteBehavior.Cascade);
//             entity.HasOne(e => e.User)
//                   .WithMany()
//                   .HasForeignKey(e => e.UserId)
//                   .OnDelete(DeleteBehavior.Restrict);
//         });
//     }
// }
//
