using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess;

public class TagConfiguration : IEntityTypeConfiguration<PostTag> 
{
    public void Configure(EntityTypeBuilder<PostTag> builder) 
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired();
    }
}