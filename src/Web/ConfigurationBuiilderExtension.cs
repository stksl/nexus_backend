
namespace Nexus.WebApi.Extensions;

public static class ConfigurationBuilderExtension
{
    public static IConfigurationBuilder AddNexusEnvironmentVariables(this IConfigurationBuilder configuration)
    {
        DotNetEnv.Env.Load();
        return configuration.AddEnvironmentVariables();
    }
}