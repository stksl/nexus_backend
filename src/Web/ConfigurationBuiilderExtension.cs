
namespace Nexus.WebApi.Extensions;

public static class ConfigurationBuilderExtension
{
    public static IConfigurationBuilder AddNexusEnvironmentVariables(this IConfigurationBuilder configuration, string envFilePath)
    {
        DotNetEnv.Env.Load(envFilePath);
        return configuration.AddEnvironmentVariables();
    }
}