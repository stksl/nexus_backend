namespace Nexus.Application;

public record EmailConfirmRequest(string UserId, string EmailToken);