namespace Nexus.Application.Auth.Dtos;

public record TokenResponse(string Value, TimeSpan Ttl);