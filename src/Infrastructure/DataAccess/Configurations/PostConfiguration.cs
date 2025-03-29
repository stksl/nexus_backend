using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostConfiguration : IEntityTypeConfiguration<Post> 
{
    public void Configure(EntityTypeBuilder<Post> builder) 
    {
        builder.ToTable("Posts");

        const int contentLength = 0b1 << 14; // 16kb

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Headline)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(contentLength);

        builder.Property(p => p.DateCreated)
            .IsRequired();

        builder.Property(p => p.LastModified)
            .IsRequired();
        
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(p => p.UserId);
    }
}