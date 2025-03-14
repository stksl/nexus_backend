using Nexus.Application.Dtos;

namespace Nexus.Application.Abstractions;

public interface IAuthService 
{
    Task Register(RegisterRequest registerRequest);
    Task<EmailConfirmResponse> ConfirmEmail(EmailConfirmRequest confirmRequest);
}