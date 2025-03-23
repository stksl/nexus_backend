using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nexus.Application;
using Nexus.Application.Abstractions;
using Nexus.Application.Auth;
using Nexus.Application.Auth.Abstractions;
using Nexus.Application.Auth.Dtos;
using Nexus.Infrastructure.DataAccess;

namespace Nexus.Infrastructure;

public class JwtTokenService : ITokenService 
{
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenRepository _tokenRepository;
    private readonly UserManager<AppUser> _userManager;
    public JwtTokenService(IConfiguration config, ITokenRepository tokenRepository, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _config = config;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    public TokenResponse GenerateAccessToken(string username, string email) 
    {
        Claim[] claims = 
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Role, AppRoles.User),
            new Claim(ClaimTypes.Email, email)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        double expiresSeconds = double.Parse(_config["JWT:s_AccessExpires"]!);
        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"], 
            claims: claims, 
            expires: DateTime.UtcNow.AddSeconds(expiresSeconds),
            signingCredentials: credentials);

        return new TokenResponse(Value: new JwtSecurityTokenHandler().WriteToken(securityToken), Ttl: TimeSpan.FromSeconds(expiresSeconds)); 
    }

    public async Task<TokenResponse> GenerateRefreshToken(int userId, DateTime? expires = null) 
    {
        RefreshToken refreshToken = await _tokenRepository.CreateRefreshToken(userId, expires);
        await _unitOfWork.SaveChangesAsync();

        TimeSpan ttl = refreshToken.Expires.Subtract(DateTime.UtcNow);

        return new TokenResponse(refreshToken.Value, ttl);
    }

    /// <summary>
    /// Revokes old refresh token and creates a new pair of tokens
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>Access and refresh token respectively</returns>
    /// <exception cref="AuthException"></exception>
    public async Task<(TokenResponse, TokenResponse)> RefreshTokens(string refreshToken) 
    {
        RefreshToken? existing = await _tokenRepository.GetRefreshTokenByValue(refreshToken);
        if (existing == null || existing.Expires < DateTime.UtcNow)
            throw new AuthException("Refresh token is invalid");

        AppUser? user = await _userManager.FindByIdAsync(existing.UserId.ToString());

        if (user == null) 
            throw new AuthException("No user with specified userId exists");

        TokenResponse accessToken = GenerateAccessToken(user.UserName!, user.Email!);

        existing.Value = Guid.NewGuid().ToString();

        await _unitOfWork.SaveChangesAsync();
        return (accessToken, new TokenResponse(existing.Value, existing.Expires.Subtract(DateTime.UtcNow)));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    /// <exception cref="AuthException"></exception>
    public async Task RevokeRefreshToken(string refreshToken) 
    {
        await _tokenRepository.DeleteRefreshToken(refreshToken);
        await _unitOfWork.SaveChangesAsync();
    }
}
