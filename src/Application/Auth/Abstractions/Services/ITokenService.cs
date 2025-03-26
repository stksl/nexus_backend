using Nexus.Application.Auth.Dtos;

namespace Nexus.Application.Auth.Abstractions;

public interface ITokenService 
{
    TokenResponse GenerateAccessToken(string id, string username, string email);
    Task<TokenResponse> GenerateRefreshToken(int userId, DateTime? expires = null);
    Task RevokeRefreshToken(string refreshToken);
    Task<(TokenResponse, TokenResponse)> RefreshTokens(string refreshToken);
}