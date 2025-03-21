using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike> 
{
    public void Configure(EntityTypeBuilder<PostLike> builder) 
    {
        builder.ToTable("PostLikes");
        
        builder.HasKey(pl => pl.Id);

        builder.HasOne<Post>()
            .WithMany()
            .HasForeignKey(pl => pl.PostId);

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(pl => pl.UserId);

    }
}