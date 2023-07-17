using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Controllers;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests;

public class BookControllerTests : BaseTests
{
    [Fact]
    public async Task CanGetBookCount()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));

        var bookService = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.GetBookCountAsync();

        // Assert
        var result = (int)response.Should().BeOfType<OkObjectResult>().Subject.Value;

        result.Should().Be(100);

        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanGetFirstTenBooks()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));

        var bookService = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.GetBooksAsync(0, 10);

        // Assert
        var result = (IEnumerable<Book>)response.Should().BeOfType<OkObjectResult>().Subject.Value;

        result.Should().BeEquivalentTo(Mapper.Map<IEnumerable<BookEntity>, IEnumerable<Book>>(books.Take(10)));

        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanSearchBooksByNameOrAuthor()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();
        var searchText = books.First().Name;
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));

        var bookService = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.GetBooksAsync(0, 100, searchText);

        // Assert
        var result = (IEnumerable<Book>)response.Should().BeOfType<OkObjectResult>().Subject.Value;

        var expected = Mapper.Map<IEnumerable<BookEntity>, IEnumerable<Book>>(books
            .Where(b => b.Name.Contains(searchText) || b.Author.Contains(searchText)));

        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }
}