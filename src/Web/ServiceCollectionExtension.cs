using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Nexus.Infrastructure;

namespace Nexus.WebApi.Extensions;

public static class ServiceCollectionExtension 
{
    public static IServiceCollection AddWebApiDependencies(this IServiceCollection services, IConfiguration config) 
    {
        // add antiforgery in the future
        return AddJwtAuthentication(services, config)
            .AddNexusAuthorization();
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config) 
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => 
        {
            options.Events = new JwtBearerEvents() 
            {
                OnMessageReceived = ctx => {
                    string accessToken = ctx.Request.Cookies["AccessToken"]!;

                    ctx.Token = accessToken;

                    return Task.CompletedTask;
                }
            }; 
            options.TokenValidationParameters = new TokenValidationParameters() 
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JWT:Issuer"],
                ValidAudience = config["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SigningKey"]!))
            };
        });
        return services;
    }
    private static IServiceCollection AddNexusAuthorization(this IServiceCollection services) 
    {
        return services.AddAuthorization(options => 
        {
            options.AddPolicy(AppRoles.User, policyBuilder => 
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireRole(AppRoles.User);
            });
            options.AddPolicy(AppRoles.Admin, policyBuilder => 
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireRole(AppRoles.Admin);
            });
        });
    }
}