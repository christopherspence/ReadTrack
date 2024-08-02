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
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests.Controllers;

public class UserControllerTests : BaseControllerTests
{
    /*[Fact]
    public async Task CanGetCurrentUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        
        var userService = new UserService(
            new Mock<ILogger<UserService>>().Object, 
            Context, 
            new Mock<IPasswordHasher<User>>().Object,
            new Mock<ITokenService>().Object, Mapper);

        var controller = new UserController(new Mock<ILogger<UserController>>().Object, userService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.GetCurrentUserAsync();

        // Assert
        var result = (User)response.Should().BeOfType<OkObjectResult>().Subject.Value;

        var expected = Mapper.Map<UserEntity, User>(user);
        expected.Password = string.Empty;

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CanRegisterUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var hasher = new PasswordHasher<User>();
        var expectedToken = Guid.NewGuid().ToString();
        var expectedUser = Mapper.Map<UserEntity, User>(user);
        var tokenType = "Bearer";

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(m => m.GenerateToken(It.IsAny<User>())).Returns(new TokenResponse
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
            hasher,
            tokenServiceMock.Object,
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

        tokenServiceMock.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CanUpdateUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var hasher = new PasswordHasher<User>();        
        
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        
        var userService = new UserService(
            new Mock<ILogger<UserService>>().Object,
            Context, 
            hasher,
            new Mock<ITokenService>().Object, 
            Mapper);

        var controller = new UserController(new Mock<ILogger<UserController>>().Object, userService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var updatedUser = Mapper.Map<UserEntity, User>(user);
        var originalPassword = updatedUser.Password;
        updatedUser.FirstName = RandomGenerator.CreateFirstName();
        updatedUser.LastName = RandomGenerator.CreateLastName();
        updatedUser.ProfilePicture = $"{Guid.NewGuid()}.jpg";
        updatedUser.Email  = RandomGenerator.CreateEmailAddress();
        updatedUser.Password = Guid.NewGuid().ToString();
        
        var response = await controller.UpdateUserAsync(updatedUser.Id, updatedUser);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        var expected = Mapper.Map<User, UserEntity>(updatedUser);
        var actual = await Context.Users.FirstAsync();
        
        actual.Should().BeEquivalentTo(expected,
            o => o.Excluding(n => n.Path.Equals("Password") ||
                                  n.Path.Equals("Books") ||
                                  n.Path.EndsWith("Created") ||
                                  n.Path.EndsWith("Modified")));

        actual.Password.Should().NotBeEquivalentTo(originalPassword);
    }*/
}