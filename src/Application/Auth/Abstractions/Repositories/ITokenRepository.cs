using Nexus.Application.Auth.Dtos;

namespace Nexus.Application.Auth.Abstractions;

public interface ITokenRepository 
{
    Task<RefreshToken> CreateRefreshToken(int userId, DateTime? expires = null);
    Task<RefreshToken?> GetRefreshTokenByValue(string tokenValue);
    Task<bool> DeleteRefreshToken(string tokenValue);
} 