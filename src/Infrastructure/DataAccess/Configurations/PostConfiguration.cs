using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostConfiguration : IEntityTypeConfiguration<Post> 
{
    public void Configure(EntityTypeBuilder<Post> builder) 
    {
        const int contentLength = 0b1 << 14; // 16kb
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Headline)
            .HasMaxLength(255)
            .IsRequired()
            .HasColumnName("headline");

        builder.Property(p => p.Content)
            .HasMaxLength(contentLength)
            .IsRequired()
            .HasColumnName("content");

        builder.Property(p => p.DateCreated)
            .HasColumnName("date_created");

        builder.Property(p => p.LastModified)
            .HasColumnName("last_modified");
        
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(p => p.UserId);
    }
}