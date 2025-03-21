using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Extensions;
using Nexus.Application.Extensions;
using Nexus.WebApi.Extensions;
using Nexus.WebApi;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWebApiDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapNexusAuthApis();
app.Map("/get", async ([FromQuery]int id, [FromServices]IMediator mediator) => 
{
    Result result = await mediator.Send(new GetPostByIdQuery(id));
    if (result.Succeed)
        System.Console.WriteLine(((Result<Post>)result).ResultValue!.Content);

}).DisableAntiforgery();

using (var scope = app.Services.CreateScope()) 
{
    using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await NexusDbContextExtension.SeedRoles(roleManager);
}
app.Run();