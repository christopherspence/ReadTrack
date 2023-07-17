using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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

    [Fact]
    public async Task CanCreateBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var book = RandomGenerator.GenerateRandomBook();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));


        var bookService = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        var controller = new BookController(new Mock<ILogger<BookController>>().Object, userServiceMock.Object, bookService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.CreateBookAsync(new CreateBookRequest
        {
            Name = book.Name,
            Author = book.Author,
            Category = book.Category,
            NumberOfPages = book.NumberOfPages,
            Published = book.Published
        });

        // Assert
        var result = (Book)response.Should().BeOfType<CreatedResult>().Subject.Value;

        result.Should().BeEquivalentTo(Mapper.Map<BookEntity, Book>(book));

        (await Context.Books.SingleAsync()).Should().BeEquivalentTo(book, o => o.Excluding(n =>
            n.Path.EndsWith("Sessions") ||
            n.Path.EndsWith("Created") ||
            n.Path.EndsWith("Modified")));

        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);    
    }

    [Fact]
    public async Task CanUpdateBook()
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
        var firstBook = Mapper.Map<BookEntity, Book>(books.First());
        firstBook.Name = RandomGenerator.CreatePlaceName();
        firstBook.Author = RandomGenerator.CreateAuthorName();
        firstBook.Published = DateTime.UtcNow.AddYears(RandomGenerator.CreateNumber(1, 30));
        firstBook.Category = (BookCategory)RandomGenerator.CreateNumber(1, 12);
        firstBook.Finished = !firstBook.Finished;

        var response = await controller.UpdateBookAsync(1, firstBook);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        var expected = Mapper.Map<Book, BookEntity>(firstBook);
        expected.UserId = user.Id;

        (await Context.Books.FirstAsync()).Should().BeEquivalentTo(expected,
            o => o.Excluding(n => n.Path.Equals("Sessions") ||
                                  n.Path.EndsWith("Created") ||
                                  n.Path.EndsWith("Modified")));


        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanDeleteBook()
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
        var response = await controller.DeleteBookAsync(books.First().Id);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        (await Context.Books.FirstAsync()).IsDeleted.Should().BeTrue();

        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }
}