using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Auth;
using Nexus.Application.Auth.Abstractions;

namespace Nexus.Infrastructure.DataAccess;

public class TokenRepository : ITokenRepository
{
    private readonly DbSet<RefreshToken> _refreshTokens;
    private readonly IConfiguration _config;
    public TokenRepository(NexusDbContext dbContext, IConfiguration config)
    {
        _refreshTokens = dbContext.RefreshTokens;
        _config = config;
    }
    public async Task<RefreshToken> CreateRefreshToken(int userId, DateTime? expires = null)
    {
        RefreshToken token = new RefreshToken() 
        {
            Expires = expires ?? DateTime.UtcNow.AddSeconds(double.Parse(_config["JWT:s_RefreshExpires"]!)),
            UserId = userId,
            Value = Guid.NewGuid().ToString() 
        };

        await _refreshTokens.AddAsync(token);
        return token;
    }
    public async Task<RefreshToken?> GetRefreshTokenByValue(string tokenValue) 
    {
        return await _refreshTokens.FirstOrDefaultAsync(r => r.Value == tokenValue);
    }
    /// <summary>
    /// Returns expiration date of a deleted token
    /// </summary>
    /// <param name="tokenValue"></param>
    /// <returns></returns>
    public async Task<bool> DeleteRefreshToken(string tokenValue) 
    {
        var token = await GetRefreshTokenByValue(tokenValue);

        if (token != null) 
        {
            _refreshTokens.Remove(token);
            return true;
        }
        
        return false;
    }
}