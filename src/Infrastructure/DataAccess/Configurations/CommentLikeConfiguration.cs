using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike> 
{
    public void Configure(EntityTypeBuilder<CommentLike> builder) 
    {
        builder.ToTable("CommentLikes");

        builder.HasKey(cl => new { cl.UserId, cl.CommentId });

        builder.HasOne<Comment>()
            .WithMany()
            .HasForeignKey(cl => cl.CommentId);
            
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(cl => cl.UserId);
    }
}