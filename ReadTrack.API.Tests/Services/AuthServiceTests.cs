using System;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;
using System.Threading.Tasks;

namespace ReadTrack.API.Tests.Services;

public class AuthServiceTests : BaseServiceTests
{
    [Fact]
    public async Task CanLogin()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();
        var hasher = new PasswordHasher<User>();
        var tokenType = "Bearer";
        var expectedToken = Guid.NewGuid().ToString();                

        var hasherMock = new Mock<IPasswordHasher<User>>();
        hasherMock.Setup(m => m.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Success);

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(m => m.GenerateToken(It.IsAny<User>())).Returns(new TokenResponse
        {
            Token = expectedToken,
            Expires = DateTime.UtcNow.AddDays(1),
            Issued = DateTime.UtcNow,
            Type = tokenType,
            User = user
        });

        var service = new AuthService(new Mock<ILogger<AuthService>>().Object,
            Context,
            hasherMock.Object,
            tokenServiceMock.Object,
            userServiceMock.Object,
            Mapper);

        // Act
        var result = await service.LoginAsync(new AuthRequest
        {
            Email = user.Email,
            Password = user.Password
        });

        // Assert
        result.Should().BeEquivalentTo(new TokenResponse
        {
            Type = tokenType,
            Token = expectedToken,
            User = user
        }, o => o.Excluding(n => 
            n.Path.EndsWith("Issued") ||
            n.Path.EndsWith("Expires") ||
            n.Path.EndsWith("User.Password")));

        hasherMock.Verify(m => m.VerifyHashedPassword(It.IsAny<User>(), user.Password, user.Password), Times.Once);
        tokenServiceMock.Verify(m => m.GenerateToken(user), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email), Times.Once);
    }
}