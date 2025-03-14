using System.Data;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Abstractions;
using Nexus.Infrastructure.DataAccess;
using Nexus.Infrastructure.Smtp;
using Npgsql;

namespace Nexus.Infrastructure;

public static class ServiceCollectionExtension 
{

    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration config) 
    {
        return services.AddTransient<IAuthService, AuthService>().AddTransient<ITokenService, JwtTokenService>() // authService, jwtService
            .AddTransient<IDbConnection>(sp => new NpgsqlConnection(config.GetConnectionString("Npgsql"))) // connection for dapper
            .AddNexusDbContext(config) // db context
            .AddNexusIdentity() // identity
            .AddRepositories() // repos
            .AddUnitOfWork() // Unit of Work
            .AddHangfireDependencies(config); // Hangfire
    }
    private static IServiceCollection AddNexusDbContext(this IServiceCollection services, IConfiguration config) 
    {
        return services.AddDbContext<NexusDbContext>(o => 
            o.UseNpgsql(config.GetConnectionString("Npgsql")));
    }
    private static IServiceCollection AddNexusIdentity(this IServiceCollection services) 
    {
        services.AddIdentity<AppUser, IdentityRole<int>>(o => 
        {
            o.Password.RequiredLength = 8;
            o.Password.RequireLowercase = true;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireDigit = false;
            o.Password.RequireUppercase = false;

            o.User.RequireUniqueEmail = true;

            o.SignIn.RequireConfirmedEmail = true;
            
        }).AddEntityFrameworkStores<NexusDbContext>()
        .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

        services.Configure<DataProtectionTokenProviderOptions>(options => 
        {
            options.TokenLifespan = TimeSpan.FromMinutes(5);
        });
        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services) 
    {
        return services.AddScoped<IPostRepository, PostRepository>()
            .AddScoped<IPostReadRepository, PostReadRepository>();
    }
    private static IServiceCollection AddUnitOfWork(this IServiceCollection services) 
    {
        return services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    private static IServiceCollection AddHangfireDependencies(this IServiceCollection services, IConfiguration config) 
    {
        return services.AddTransient<IEmailSender, EmailSender>()
            .AddHangfire(c => 
                c.UsePostgreSqlStorage(o => o.UseNpgsqlConnection(config.GetConnectionString("Hangfire"))))
            .AddHangfireServer();
    }
}