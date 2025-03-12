using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Domain.Entities;
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
app.Map("/get", async ([FromQuery]int id, [FromServices]IMediator mediator) => 
{
    Result result = await mediator.Send(new GetPostByIdQuery(id));
    if (result.Succeed)
        System.Console.WriteLine(((Result<Post>)result).ResultValue!.Content);

}).DisableAntiforgery();

app.Run();