using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared.Models;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;
using ReadTrack.Shared.Models.Requests;

namespace ReadTrack.API.Tests.Services;

public class UserServiceTests : BaseServiceTests
{
    [Fact]
    public async Task CanGetUserById()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        
        var service = new UserService(
            new Mock<ILogger<UserService>>().Object, 
            Context, 
            new Mock<IPasswordHasher<User>>().Object,
            new Mock<ITokenService>().Object, Mapper);

        // Act
        var result = await service.GetUserAsync(user.Id);

        // Assert
        var expected = Mapper.Map<UserEntity, User>(user);
        expected.Password = string.Empty;

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CanGetUserByEmail()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        
        var service = new UserService(
            new Mock<ILogger<UserService>>().Object, 
            Context, 
            new Mock<IPasswordHasher<User>>().Object,
            new Mock<ITokenService>().Object, Mapper);

        // Act
        var result = await service.GetUserByEmailAsync(user.Email);

        // Assert
        var expected = Mapper.Map<UserEntity, User>(user);
        
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CanCreateUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();
        var expectedToken = Guid.NewGuid().ToString();        
        var tokenType = "Bearer";
        
        var hasherMock = new Mock<IPasswordHasher<User>>();
        hasherMock.Setup(m => m.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(RandomGenerator.CreatePlaceName());

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(m => m.GenerateToken(It.IsAny<User>())).Returns(new TokenResponse
        {
            Token = expectedToken,
            Expires = DateTime.UtcNow.AddDays(1),
            Issued = DateTime.UtcNow,
            Type = tokenType,
            User = user
        });

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object, 
            Context, 
            hasherMock.Object,
            tokenServiceMock.Object,
            Mapper);
        
        // Act
        var result = await service.CreateUserAsync(new CreateUserRequest
        {
            Email = user.Email,
            Password = user.Password,
            FirstName = user.FirstName,
            LastName = user.LastName            
        });

        result.Should().BeEquivalentTo(new TokenResponse
        {
            Type = tokenType,
            User = user
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

        hasherMock.Verify(m => m.HashPassword(It.IsAny<User>(), user.Password), Times.Once);
        tokenServiceMock.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CanUpdateUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var hasherMock = new Mock<IPasswordHasher<User>>();        
        
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        
        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            Context, 
            hasherMock.Object,
            new Mock<ITokenService>().Object, 
            Mapper);

        // Act
        var updatedUser = Mapper.Map<UserEntity, User>(user);
        var originalPassword = updatedUser.Password;
        updatedUser.FirstName = RandomGenerator.CreateFirstName();
        updatedUser.LastName = RandomGenerator.CreateLastName();
        updatedUser.ProfilePicture = $"{Guid.NewGuid()}.jpg";
        updatedUser.Email  = RandomGenerator.CreateEmailAddress();
        updatedUser.Password = Guid.NewGuid().ToString();
        
        var response = await service.UpdateUserAsync(updatedUser.Id, updatedUser);

        // Assert
        response.Should().BeTrue();

        var expected = Mapper.Map<User, UserEntity>(updatedUser);
        var actual = await Context.Users.FirstAsync();
        
        actual.Should().BeEquivalentTo(expected,
            o => o.Excluding(n => n.Path.Equals("Password") ||
                                  n.Path.Equals("Books") ||
                                  n.Path.EndsWith("Created") ||
                                  n.Path.EndsWith("Modified")));

        actual.Password.Should().NotBeEquivalentTo(originalPassword);

        hasherMock.Verify(m => m.HashPassword(It.IsAny<User>(), updatedUser.Password), Times.Once);
    }
}