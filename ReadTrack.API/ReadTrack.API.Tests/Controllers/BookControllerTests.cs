using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Controllers;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests.Controllers;

public class BookControllerTests : BaseControllerTests
{
    [Fact]
    public async Task CanGetBookCount()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var count = RandomGenerator.CreateNumber(1, 100);
        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBookCountAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(count);
        
        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var searchText = RandomGenerator.CreatePlaceName();
        var response = await controller.GetBookCountAsync(searchText);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        bookServiceMock.Verify(m => m.GetBookCountAsync(user.Id, searchText), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email), Times.Once);        
    }

    [Fact]
    public async Task CanGetBooks()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBooksAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new List<Book>());

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var offset = RandomGenerator.CreateNumber(1, 100);
        var count = RandomGenerator.CreateNumber(1, 100);
        var searchText = RandomGenerator.CreatePlaceName();
        var response = await controller.GetBooksAsync(offset, count, searchText);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        bookServiceMock.Verify(m => m.GetBooksAsync(user.Id, offset, count, searchText));
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email), Times.Once);
    }

    [Fact]
    public async Task CanGetBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Book());

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var bookId = RandomGenerator.CreateNumber(1, 100);
        var response = await controller.GetBookAsync(bookId);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        bookServiceMock.Verify(m => m.GetBookAsync(user.Id, bookId), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email), Times.Once);
    }

    [Fact]
    public async Task CanCreateBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.CreateBookAsync(It.IsAny<int>(), It.IsAny<CreateBookRequest>())).ReturnsAsync(new Book());

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var request = RandomGenerator.GenerateRandomCreateBookRequest();
        var response = await controller.CreateBookAsync(request);

        // Assert
        response.Should().BeOfType<CreatedResult>();

        bookServiceMock.Verify(m => m.CreateBookAsync(user.Id, request), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email), Times.Once);    
    }

    [Fact]
    public async Task CanUpdateBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.UpdateBookAsync(It.IsAny<int>(), It.IsAny<Book>())).ReturnsAsync(true);

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var book = RandomGenerator.GenerateRandomBookModel();
        book.Name = RandomGenerator.CreatePlaceName();
        book.Author = RandomGenerator.CreateAuthorName();
        book.Published = DateTime.UtcNow.AddYears(RandomGenerator.CreateNumber(1, 30));
        book.Category = (BookCategory)RandomGenerator.CreateNumber(1, 12);
        book.Finished = !book.Finished;

        var response = await controller.UpdateBookAsync(1, book);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        bookServiceMock.Verify(m => m.UpdateBookAsync(user.Id, book));
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanDeleteBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.DeleteBookAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var bookId = RandomGenerator.CreateNumber(1, 100);
        var response = await controller.DeleteBookAsync(bookId);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        bookServiceMock.Verify(m => m.DeleteBookAsync(user.Id, bookId), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }
}