using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Domain.Entities;
using Nexus.Infrastructure;
using Nexus.Application.Abstractions;
using Nexus.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// убрать коммент в PostConfiguration.cs
builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies();

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.Map("/get", async ([FromQuery]int id, [FromServices]IMediator mediator) => 
{
    Result result = await mediator.Send(new GetPostByIdQuery(id));
    if (result.Succeed)
        System.Console.WriteLine(((Result<Post>)result).ResultValue!.Content);

}).DisableAntiforgery();
app.MapPost("/register", async ([FromBody]UserRegisterRequest registerRequest, [FromServices]IAuthService authService) => 
{
    await authService.Register(registerRequest);
}).DisableAntiforgery();
app.MapGet("/api/confirm", async ([FromQuery]string id, [FromQuery]string token, [FromServices]IAuthService authService) => 
{
    await authService.ConfirmEmail(new EmailConfirmRequest(id, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token))));
}).DisableAntiforgery();
app.Run();