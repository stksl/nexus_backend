using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Abstractions;
using Nexus.Infrastructure.DataAccess;

namespace Nexus.Infrastructure;

public static class ServiceCollectionExtension 
{

    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration config) 
    {
        return AddNexusDbContext(services, config)
            .AddNexusIdentity()
            .AddRepositories()
            .AddUnitOfWork();
    }
    private static IServiceCollection AddNexusDbContext(this IServiceCollection services, IConfiguration config) 
    {
        return services.AddDbContext<NexusDbContext>(o => 
            o.UseNpgsql(config.GetConnectionString("Npgsql")));
    }
    private static IServiceCollection AddNexusIdentity(this IServiceCollection services) 
    {
        services.AddIdentity<AppUser, AppRole>(o => 
        {
            o.Password.RequiredLength = 8;
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            
        }).AddEntityFrameworkStores<NexusDbContext>();
        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services) 
    {
        return services.AddScoped<IPostRepository, PostRepository>();
    }
    private static IServiceCollection AddUnitOfWork(this IServiceCollection services) 
    {
        return services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}