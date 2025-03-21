using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostRespostConfiguration : IEntityTypeConfiguration<PostRepost> 
{
    public void Configure(EntityTypeBuilder<PostRepost> builder) 
    {
        builder.ToTable("PostReposts");

        builder.HasKey(pl => pl.Id);

        builder.HasOne<Post>()
            .WithMany()
            .HasForeignKey(pl => pl.PostId);

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(pl => pl.UserId);
    }
}