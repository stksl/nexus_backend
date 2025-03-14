using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Application.Extensions;

public static class ServiceCollectionExtension 
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services) 
    {
        return AddAutoMapperWithProfiles(services).AddMediatr().AddFluentValidator();
    }

    private static IServiceCollection AddAutoMapperWithProfiles(this IServiceCollection services) 
    {
        return services.AddAutoMapper(typeof(PostProfile));
    }
    private static IServiceCollection AddMediatr(this IServiceCollection services) 
    {
        return services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));
    }
    private static IServiceCollection AddFluentValidator(this IServiceCollection services) 
    {
        return services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
    }
}