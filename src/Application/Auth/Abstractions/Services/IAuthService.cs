using System.Net.Mail;
using Nexus.Application.Auth.Dtos;

namespace Nexus.Application.Auth.Abstractions;

public interface IAuthService 
{
    Task Register(RegisterRequest registerRequest);
    Task<AuthenticationResponse> ConfirmEmail(EmailConfirmRequest confirmRequest);
    Task<AuthenticationResponse> Login(LoginRequest loginRequest);
    Task<AuthenticationResponse> LoginGoogle(MailAddress email);
    Task<bool> Logout(string refreshToken);
}