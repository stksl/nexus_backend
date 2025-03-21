using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Nexus.Application.Auth.Abstractions;
using Nexus.Application.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Runtime.InteropServices;
using Nexus.Infrastructure;
namespace Nexus.WebApi;

public static class AuthApis
{
    public static void MapNexusAuthApis(this WebApplication app)
    {
        var authApi = app.MapGroup("/api/auth");

        authApi.MapPost("/register", async (
            [FromBody] RegisterRequest registerRequest,
            [FromServices] IAuthService authService,
            [FromServices] IValidator<RegisterRequest> validator) =>
        {
            var validationResult = validator.Validate(registerRequest);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            await authService.Register(registerRequest);
            return Results.Ok();
        });

        authApi.MapGet("/confirm", async (
            [FromQuery] string id,
            [FromQuery] string token,
            [FromServices] IAuthService authService,
            HttpContext context) =>
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            
            var response = await authService.ConfirmEmail(new EmailConfirmRequest(id, token));
            AddAccessTokenCookie(context, response.AccessToken);
            AddRefreshTokenCookie(context, response.RefreshToken);
        });

        authApi.MapPost("/login", async (
            [FromBody] LoginRequest loginRequest,
            [FromServices] IAuthService authService,
            [FromServices] IValidator<LoginRequest> validator,
            HttpContext context) =>
        {
            var result = validator.Validate(loginRequest);
            if (!result.IsValid)
                Results.BadRequest(result.Errors);

            var response = await authService.Login(loginRequest);
            AddAccessTokenCookie(context, response.AccessToken);
            AddRefreshTokenCookie(context, response.RefreshToken);
        });

        authApi.MapGet("/refresh", async (HttpContext context, [FromServices]ITokenService tokenService) => 
        {
            string? refreshToken = context.Request.Cookies["RefreshToken"];
            if (refreshToken == null)
                return Results.BadRequest("Login first!");
            
            (TokenResponse, TokenResponse) accessRefreshPair = await tokenService.RefreshTokens(refreshToken);

            AddAccessTokenCookie(context, accessRefreshPair.Item1);
            AddRefreshTokenCookie(context, accessRefreshPair.Item2);
            
            return Results.Ok();
        });

        authApi.MapGet("/test",  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]async (HttpContext context, ClaimsPrincipal user) => 
        {
            await context.Response.WriteAsync("asdasdasdasd");
        });
    }

    private static void AddAccessTokenCookie(HttpContext context, TokenResponse accessToken)
    {
        context.Response.Cookies.Append("AccessToken", accessToken.Value, new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = false, // add secure in the future
            Expires = DateTimeOffset.UtcNow.Add(accessToken.Ttl)
        });
    }
    private static void AddRefreshTokenCookie(HttpContext context, TokenResponse refreshToken)
    {
        context.Response.Cookies.Append("RefreshToken", refreshToken.Value, new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = false, // add secure in the future
            Expires = DateTimeOffset.UtcNow.Add(refreshToken.Ttl)
        });
    }
}