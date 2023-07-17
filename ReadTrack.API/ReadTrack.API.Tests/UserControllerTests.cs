using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Controllers;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests;

public class UserControllerTests : BaseTests
{
    [Fact]
    public async Task CanRegisterUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var hasher = new PasswordHasher<User>();
        var expectedToken = Guid.NewGuid().ToString();
        var expectedUser = Mapper.Map<UserEntity, User>(user);
        var tokenType = "Bearer";

        var authServiceMock = new Mock<IAuthService>();
        authServiceMock.Setup(m => m.LoginAsync(It.IsAny<AuthRequest>())).ReturnsAsync(new TokenResponse
        {
            Token = expectedToken,
            Expires = DateTime.UtcNow.AddDays(1),
            Issued = DateTime.UtcNow,
            Type = tokenType,
            User = expectedUser
        });

        var userService = new UserService(
            new Mock<ILogger<UserService>>().Object, 
            Context, 
            authServiceMock.Object,
            Mapper);

        var controller = new UserController(new Mock<ILogger<UserController>>().Object, userService);

        // Act
        var response = await controller.RegisterAsync(new CreateUserRequest
        {
            Email = user.Email,
            Password = user.Password,
            FirstName = user.FirstName,
            LastName = user.LastName            
        });

        var result = (TokenResponse)response.Should().BeOfType<CreatedResult>().Subject.Value;

        result.Should().BeEquivalentTo(new TokenResponse
        {
            Type = tokenType,
            User = expectedUser
        }, o => o.Excluding(n =>
            n.Path.EndsWith("Token") ||
            n.Path.EndsWith("Issued") ||
            n.Path.EndsWith("Expires")));

        var actual = await Context.Users.SingleAsync();
        actual.Should().BeEquivalentTo(user, o => o.Excluding(
            n => n.Path.EndsWith("Password") ||
                 n.Path.EndsWith("ProfilePicture") ||
                 n.Path.EndsWith("Created") ||
                 n.Path.EndsWith("Modified")));

        authServiceMock.Verify(m => m.LoginAsync(It.IsAny<AuthRequest>()), Times.Once);
    }
}