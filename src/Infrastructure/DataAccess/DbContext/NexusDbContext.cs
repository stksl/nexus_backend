using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Auth;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class NexusDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int> 
{
    public NexusDbContext()
    {
        
    }
    public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options)
    {
        
    }
    public DbSet<Post> Posts {get; set;}
    public DbSet<PostTag> PostTags {get; set;}
    public DbSet<Comment> Comments {get; set;}
    public DbSet<Tag> Tags {get; set;}
    public DbSet<PostLike> PostLikes {get; set;}
    public DbSet<PostRepost> PostReposts {get; set;}
    public DbSet<CommentLike> CommentLikes {get; set;}
    public DbSet<RefreshToken> RefreshTokens {get; set;}
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);
    }
}