namespace Nexus.WebApi.Extensions;

public static class ServiceCollectionExtension 
{
    public static IServiceCollection AddWebApiDependencies(this IServiceCollection services) 
    {
        // add antiforgery in the future
        return services;
    }
}