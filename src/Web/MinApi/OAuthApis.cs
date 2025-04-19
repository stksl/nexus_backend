using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Nexus.Application.Auth.Abstractions;
using Nexus.Infrastructure.DataAccess;
using System.Security.Claims;
using System.Net.Mail;

namespace Nexus.WebApi;

public static partial class AuthApis 
{
    public static void MapNexusOAuthApis(this WebApplication app) 
    {
        var oauthApi = app.MapGroup("/api/oauth");

        oauthApi.MapGet("google-signin", (LinkGenerator linkGenerator, HttpContext ctx, 
            SignInManager<AppUser> signInManager) => 
        {  
            var properties = signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, 
                linkGenerator.GetUriByName(ctx, "GoogleCallback"));
            return Results.Challenge(properties, [GoogleDefaults.AuthenticationScheme]);
        });

        oauthApi.MapGet("google-callback", async (HttpContext ctx, IAuthService authService) => 
        {
            var result = await ctx.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return Results.Unauthorized();

            string? email = result.Principal.FindFirstValue(ClaimTypes.Email);

            if (email == null || !MailAddress.TryCreate(email, out MailAddress? mailAddress))
                return Results.Unauthorized();

            var response = await authService.LoginGoogle(mailAddress);

            AddAccessTokenCookie(ctx, response.AccessToken);
            AddRefreshTokenCookie(ctx, response.RefreshToken);

            return Results.Ok();
        }).WithName("GoogleCallback");
    }
}