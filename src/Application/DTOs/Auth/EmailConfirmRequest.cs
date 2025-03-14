namespace Nexus.Application.Dtos;

public record EmailConfirmRequest(string UserId, string EmailToken);