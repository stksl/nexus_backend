using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Nexus.Infrastructure.DataAccess;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NexusDbContext>
{
    private IConfiguration config;
    public DesignTimeDbContextFactory()
    {
        ConfigurationBuilder cb = new ConfigurationBuilder();

        cb.AddUserSecrets(typeof(NexusDbContext).Assembly);

        config = cb.Build();
    }
    public NexusDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<NexusDbContext>();

        builder.UseNpgsql(config.GetConnectionString("Npgsql"));

        return new NexusDbContext(builder.Options);
    }
}