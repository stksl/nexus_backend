using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class TagConfiguration : IEntityTypeConfiguration<Tag> 
{
    public void Configure(EntityTypeBuilder<Tag> builder) 
    {
        builder.ToTable("Tags");

        builder.HasKey(t => t.Id);
        
        builder.HasIndex(t => t.Name).IsUnique();
    }
}