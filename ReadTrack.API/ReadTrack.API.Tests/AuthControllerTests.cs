using System;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

public class AuthControllerTests : BaseTests
{
    [Fact]
    public async void CanLogin()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();        
        var hasher = new PasswordHasher<User>();
        var tokenType = "Bearer";
        var expectedToken = Guid.NewGuid().ToString();
        var expectedUser = Mapper.Map<UserEntity, User>(user);
        
        user.Password = hasher.HashPassword(expectedUser, expectedUser.Password);

        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        var userService = new UserService(new Mock<ILogger<UserService>>().Object,
            Context,
            new Mock<IPasswordHasher<User>>().Object,
            new Mock<ITokenService>().Object,
            Mapper);

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(m => m.GenerateToken(It.IsAny<User>())).Returns(new TokenResponse
        {
            Token = expectedToken,
            Expires = DateTime.UtcNow.AddDays(1),
            Issued = DateTime.UtcNow,
            Type = tokenType,
            User = expectedUser
        });

        var authService = new AuthService(new Mock<ILogger<AuthService>>().Object,
            Context,
            hasher,
            tokenServiceMock.Object,
            userService,
            Mapper);

        var controller = new AuthController(new Mock<ILogger<AuthController>>().Object, authService);

        // Act
        var response = await controller.LoginAsync(new AuthRequest
        {
            Email = expectedUser.Email,
            Password = expectedUser.Password
        });

        // Assert
        var result = (TokenResponse)response.Should().BeOfType<CreatedResult>().Subject.Value;

        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(new TokenResponse
        {
            Type = "Bearer",
            Token = expectedToken,
            User = Mapper.Map<UserEntity, User>(user)
        }, o => o.Excluding(n => 
            n.Path.EndsWith("Issued") ||
            n.Path.EndsWith("Expires") ||
            n.Path.EndsWith("User.Password")));

        tokenServiceMock.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Once);
    }
}