namespace Nexus.Application.Abstractions;

public interface IAuthService 
{
    Task Register(UserRegisterRequest registerRequest);
    Task<EmailConfirmResponse> ConfirmEmail(EmailConfirmRequest confirmRequest);
}