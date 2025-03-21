namespace Nexus.Application.Auth.Dtos;

public record AuthenticationResponse(TokenResponse AccessToken, TokenResponse RefreshToken);