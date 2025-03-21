namespace Nexus.Application.Auth.Dtos;

public record EmailConfirmRequest(string UserId, string EmailToken);