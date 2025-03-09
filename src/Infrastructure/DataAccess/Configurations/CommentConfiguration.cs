using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class CommentConfiguration : IEntityTypeConfiguration<Comment> 
{
    public void Configure(EntityTypeBuilder<Comment> builder) 
    {
        const int contentLength = 0b1 << 12; // 4kb
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Content)
            .HasMaxLength(contentLength)
            .IsRequired();

        builder.Property(c => c.DateCreated)
            .IsRequired();

        builder.Property(c => c.LastModified)
            .IsRequired();

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId);

        builder.HasOne<Post>()
            .WithMany()
            .HasForeignKey(c => c.PostId);
    }
}