namespace Nexus.Application.Abstractions;

public interface ITokenService 
{
    string GenerateAccessToken(string username, string email);
    string GenerateRefreshToken();
}