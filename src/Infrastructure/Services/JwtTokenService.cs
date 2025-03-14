using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nexus.Application.Abstractions;

namespace Nexus.Infrastructure;

public class JwtTokenService : ITokenService 
{
    private readonly IConfiguration _config;
    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }
    public string GenerateAccessToken(string username, string email) 
    {
        Claim[] claims = 
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Role, AppRoles.User),
            new Claim(ClaimTypes.Email, email)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"], 
            claims: claims, 
            expires: DateTime.Now.AddSeconds(double.Parse(_config["JWT:s_AccessExpires"]!)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken); 
    }

    public string GenerateRefreshToken() 
    {
        return Guid.NewGuid().ToString();
    }
}
