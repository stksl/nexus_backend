using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Application.Auth;

namespace Nexus.Infrastructure.DataAccess;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken> 
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder) 
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Value)
            .IsRequired()
            .HasColumnName("value");

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(r => r.IsRevoked)
            .HasColumnName("is_revoked");

        builder.Property(r => r.Expires)
            .IsRequired()
            .HasColumnName("expires_date");
    }
}