using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Infrastructure;

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
app.MapPost("/create", async ([FromForm]CreatePostCommand createPostCommand, [FromServices]IMediator mediator) => 
{
    Result result = await mediator.Send(createPostCommand);

    System.Console.WriteLine(result.Succeed);
}).DisableAntiforgery();

app.Run();