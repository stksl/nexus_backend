using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostTagConfiguration : IEntityTypeConfiguration<PostTag> 
{
    public void Configure(EntityTypeBuilder<PostTag> builder) 
    {
        builder.ToTable("PostTags");

        builder.HasKey(pt => new { pt.PostId, pt.TagId });

        builder.HasOne<Post>()
            .WithMany()
            .HasForeignKey(pt => pt.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}