namespace Nexus.Application.Auth.Dtos;

public record LoginRequest(string Identifier, string Password, bool IsEmailIdentifier);