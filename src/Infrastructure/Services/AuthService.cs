using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Nexus.Application;
using Nexus.Application.Abstractions;
using Nexus.Application.Dtos;
using Nexus.Infrastructure.DataAccess;

namespace Nexus.Infrastructure;

public class AuthService : IAuthService 
{
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _config;
    public AuthService(ITokenService tokenService, 
        SignInManager<AppUser> signInManager, 
        UserManager<AppUser> userManager,
        IConfiguration config)
    {
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userManager = userManager;
        _config = config;
    }

    public async Task Register(RegisterRequest registerRequest) 
    {
        if (await _userManager.FindByEmailAsync(registerRequest.Email) != null)
            throw new AuthException("User with the same email already exists!");
        if (await _userManager.FindByNameAsync(registerRequest.Username) != null)
            throw new AuthException("User with the same name already exists!");

        AppUser user = new AppUser() 
        {
            Email = registerRequest.Email,
            UserName = registerRequest.Username,
        };
        IdentityResult createResult = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!createResult.Succeeded) 
        {
            throw new AuthException(string.Join('\n', createResult.Errors.Select(e => e.Description)));
        }

        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, AppRoles.User);

        if (!roleResult.Succeeded) 
        {
            throw new AuthException(string.Join('\n', roleResult.Errors.Select(e => e.Description)));
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string verificationBody = 
            $"Click <a href=\"{_config["AppUrl"]}/api/confirm?id={user.Id}&token={encodedToken}\">here</a> to verify your email.";

        BackgroundJob.Enqueue<IEmailSender>(emailSender => 
            emailSender.SendMailAsync(registerRequest.Email, "Email Verification", verificationBody));
    }

    public async Task<EmailConfirmResponse> ConfirmEmail(EmailConfirmRequest confirmRequest) 
    {
        AppUser? user = await _userManager.FindByIdAsync(confirmRequest.UserId);
        if (user == null)
            throw new ArgumentException("No such user with the specified id!");
        
        IdentityResult result = await _userManager.ConfirmEmailAsync(user, confirmRequest.EmailToken);

        if (!result.Succeeded)
            throw new ArgumentException(string.Join('\n', result.Errors.Select(e => e.Description)));

        string accessToken = _tokenService.GenerateAccessToken(user.UserName!, user.Email!);
        string refreshToken = _tokenService.GenerateRefreshToken();

        return new EmailConfirmResponse(accessToken, refreshToken);
    }
}