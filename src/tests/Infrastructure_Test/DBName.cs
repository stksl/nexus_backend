global using static Nexus.Tests.InMemoryDb;

namespace Nexus.Tests;

public static class InMemoryDb 
{
    public static string RandomDbName => Guid.NewGuid().ToString();
}