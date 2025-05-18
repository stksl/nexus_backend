using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Nexus.Application;
using Nexus.Application.Auth.Dtos;
using Nexus.Infrastructure;
using Nexus.Infrastructure.DataAccess;

namespace Nexus.Tests;

public class AuthTest
{
    private Mock<UserManager<AppUser>> userManagerMock
        = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
    private Mock<IHttpContextAccessor> httpAccessorMock = new Mock<IHttpContextAccessor>();
    private Mock<IBackgroundJobClient> backgroundJobClientMock = new Mock<IBackgroundJobClient>();
    [Fact]
    public async Task AuthRegister_Test()
    {
        RegisterRequest registerRequest = new RegisterRequest("username", "e@mail.com", "pass123");

        userManagerMock.Setup(userManager =>
            userManager.CreateAsync(
                It.Is<AppUser>(u => u.Email == registerRequest.Email && u.UserName == registerRequest.Username),
                registerRequest.Password
            )
        ).ReturnsAsync(IdentityResult.Success);

        userManagerMock.Setup(userManager =>
            userManager.AddToRoleAsync(
                It.Is<AppUser>(u => u.Email == registerRequest.Email && u.UserName == registerRequest.Username),
                AppRoles.User
            )
        ).ReturnsAsync(IdentityResult.Success);

        userManagerMock.Setup(userManager =>
            userManager.GenerateEmailConfirmationTokenAsync(
                It.Is<AppUser>(u => u.Email == registerRequest.Email && u.UserName == registerRequest.Username)
            )
        ).ReturnsAsync(string.Empty);


        AuthService authService = new AuthService(null!, userManagerMock.Object, httpAccessorMock.Object, backgroundJobClientMock.Object);

        try
        {
            await authService.Register(registerRequest);
        }
        catch (AuthException ex)
        {
            Assert.Fail("Didn't expect an exception, but got one: " + ex.Message);
        }

        backgroundJobClientMock.Verify(x => 
            x.Create(
                It.Is<Job>(job => job.Method.Name == "SendMailAsync" && job.Args[0].ToString() == registerRequest.Email), 
                It.IsAny<EnqueuedState>()
            )
        );
    }
}