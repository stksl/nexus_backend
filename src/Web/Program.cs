using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Extensions;
using Nexus.Application.Extensions;
using Nexus.WebApi.Extensions;
using Nexus.WebApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo() 
    {
        Title = "Nexus backend api",
        Version = "v1",
        Description = "Backend API for the Nexus project",

        License = new OpenApiLicense() 
        {
            Name = "MIT License",
            Url = new Uri("https://mit-license.org/")
        }
    });

/*     var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); */
});
builder.Services.AddWebApiDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/* app.UseHttpsRedirection();
 */
app.UseAuthentication();
app.UseAuthorization();

app.MapNexusAuthApis();
app.MapNexusOAuthApis();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await NexusDbContextExtension.SeedRoles(roleManager);
}
app.Run();