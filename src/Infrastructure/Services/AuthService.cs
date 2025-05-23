using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Nexus.Application;
using Nexus.Application.Auth.Abstractions;
using Nexus.Application.Abstractions;
using Nexus.Application.Auth.Dtos;
using Nexus.Infrastructure.DataAccess;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

namespace Nexus.Infrastructure;

public class AuthService : IAuthService 
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBackgroundJobClient _backgroundJobClient;
    public AuthService(ITokenService tokenService,
        UserManager<AppUser> userManager,
        IHttpContextAccessor contextAccessor, 
        IBackgroundJobClient backgroundJobClient)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _backgroundJobClient = backgroundJobClient;
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

        HttpRequest? request = _contextAccessor.HttpContext?.Request;
        string appUrl = $"{request?.Scheme}://{request?.Host.Value}";

        string verificationBody = 
            $"Click <a href=\"{appUrl}/api/auth/confirm?id={user.Id}&token={encodedToken}\">here</a> to verify your email.";

        _backgroundJobClient.Enqueue<IEmailSender>(emailSender => 
            emailSender.SendMailAsync(registerRequest.Email, "Email Verification", verificationBody));
    }

    public async Task<AuthenticationResponse> ConfirmEmail(EmailConfirmRequest confirmRequest) 
    {
        AppUser? user = await _userManager.FindByIdAsync(confirmRequest.UserId);
        if (user == null)
            throw new ArgumentException("No such user with the specified id!");
        
        IdentityResult result = await _userManager.ConfirmEmailAsync(user, confirmRequest.EmailToken);

        if (!result.Succeeded)
            throw new ArgumentException(string.Join('\n', result.Errors.Select(e => e.Description)));

        TokenResponse accessToken = _tokenService.GenerateAccessToken(user.Id.ToString(), user.UserName!, user.Email!);
        TokenResponse refreshToken = await _tokenService.GenerateRefreshToken(user.Id);

        return new AuthenticationResponse(accessToken, refreshToken);
    }

    public async Task<AuthenticationResponse> Login(LoginRequest loginRequest) 
    {
        AppUser? user = loginRequest.IsEmailIdentifier ? 
            await _userManager.FindByEmailAsync(loginRequest.Identifier) : 
            await _userManager.FindByNameAsync(loginRequest.Identifier);
                
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            throw new AuthException("Password or " + (loginRequest.IsEmailIdentifier ? "email" : "username") + " are not valid!");
        
        TokenResponse accessToken = _tokenService.GenerateAccessToken(user.Id.ToString(), user.UserName!, user.Email!);
        TokenResponse refreshToken = await _tokenService.GenerateRefreshToken(user.Id);

        return new AuthenticationResponse(accessToken, refreshToken);
    }
    public async Task<AuthenticationResponse> LoginGoogle(MailAddress email) 
    {
        AppUser? existing = await _userManager.FindByEmailAsync(email.Address);
        if (existing == null) 
        {
            existing = new AppUser() 
            {
                Email = email.Address,
                UserName = "user_rnd" + Random.Shared.Next()
            };
            var result = await _userManager.CreateAsync(existing);
            if (!result.Succeeded)
                throw new AuthException(string.Join('\n', result.Errors.Select(e => e.Description)));

            result = await _userManager.AddToRoleAsync(existing, AppRoles.User);
            if (!result.Succeeded)
                throw new AuthException(string.Join('\n', result.Errors.Select(e => e.Description)));

            result = await _userManager.ConfirmEmailAsync(existing, await _userManager.GenerateEmailConfirmationTokenAsync(existing));
            if (!result.Succeeded)
                throw new AuthException(string.Join('\n', result.Errors.Select(e => e.Description)));

        }
        TokenResponse accessToken = _tokenService.GenerateAccessToken(existing.Id.ToString(), existing.UserName!, existing.Email!);
        TokenResponse refreshToken = await _tokenService.GenerateRefreshToken(existing.Id);

        return new AuthenticationResponse(accessToken, refreshToken);
    }
    public async Task<bool> Logout(string refreshToken) 
    {
        await _tokenService.RevokeRefreshToken(refreshToken);
        return true;
    }
}