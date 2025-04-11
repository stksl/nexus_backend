using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostRespostConfiguration : IEntityTypeConfiguration<PostRepost> 
{
    public void Configure(EntityTypeBuilder<PostRepost> builder) 
    {
        builder.ToTable("PostReposts");

        builder.HasKey(pr => new { pr.UserId, pr.PostId });

        builder.HasOne<Post>()
            .WithMany()
            .HasForeignKey(pr => pr.PostId);

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(pr => pr.UserId);
        
        builder.Property(pr => pr.RepostDate)
            .IsRequired();
    }
}