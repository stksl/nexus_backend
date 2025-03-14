using Microsoft.AspNetCore.Mvc;
using Nexus.Application.Dtos;
using Nexus.Application.Abstractions;

namespace Nexus.WebApi;

public static class AuthApis 
{
    public static void MapNexusAuthApis(this IEndpointRouteBuilder app) 
    {
        var authApi = app.MapGroup("/api/auth");

        authApi.MapPost("/register", async (
            [FromBody]RegisterRequest registerRequest, 
            [FromServices]IAuthService authService) => 
        {
        }).DisableAntiforgery();
    }
}