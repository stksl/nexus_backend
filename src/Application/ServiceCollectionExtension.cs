using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Application;

public static class ServiceCollectionExtension 
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services) 
    {
        return AddAutoMapperWithProfiles(services).AddMediatr();
    }

    private static IServiceCollection AddAutoMapperWithProfiles(this IServiceCollection services) 
    {
        return services.AddAutoMapper(typeof(PostProfile));
    }
    private static IServiceCollection AddMediatr(this IServiceCollection services) 
    {
        return services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));
    }
}