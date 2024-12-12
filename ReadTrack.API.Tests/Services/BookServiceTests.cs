using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests.Services;

public class BookServiceTests : BaseServiceTests
{
    [Fact]
    public async Task CanGetBookCount()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();
        
        var service = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetBookCountAsync(user.Id);

        // Assert
        result.Should().Be(100);
    }

    [Fact]
    public async Task CanGetFirstTenBooks()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();

        var service = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetBooksAsync(user.Id, 0, 10, string.Empty);

        // Assert
        result.Should().BeEquivalentTo(Mapper.Map<IEnumerable<BookEntity>, IEnumerable<Book>>(books.Take(10)));
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

        var service = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetBooksAsync(user.Id, 0, 100, searchText);

        // Assert        
        var expected = Mapper.Map<IEnumerable<BookEntity>, IEnumerable<Book>>(books
            .Where(b => b.Name.Contains(searchText) || b.Author.Contains(searchText)));

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CanGetBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var book = RandomGenerator.GenerateRandomBook();
        await Context.Books.AddAsync(book);
        await Context.SaveChangesAsync();
        
        var service = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetBookAsync(user.Id, book.Id);

        // Assert
        var expected = Mapper.Map<BookEntity, Book>(book);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CanCreateBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        
        var service = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        // Act
        var request = RandomGenerator.GenerateRandomCreateBookRequest();
        var result = await service.CreateBookAsync(user.Id, request);

        // Assert
        var book = new BookEntity
        {
            Id = 1,
            Name = request.Name,
            Author = request.Author,
            Category = request.Category,
            NumberOfPages = request.NumberOfPages,
            Published = request.Published,
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        result.Should().BeEquivalentTo(Mapper.Map<BookEntity, Book>(book));

        (await Context.Books.SingleAsync()).Should().BeEquivalentTo(book, o => o.Excluding(n =>
            n.Path.EndsWith("Sessions") ||
            n.Path.EndsWith("Created") ||
            n.Path.EndsWith("Modified")));
    }

    [Fact]
    public async Task CanUpdateBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();

        var service = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        // Act
        var firstBook = Mapper.Map<BookEntity, Book>(books.First());
        firstBook.Name = RandomGenerator.CreatePlaceName();
        firstBook.Author = RandomGenerator.CreateAuthorName();
        firstBook.Published = DateTime.UtcNow.AddYears(RandomGenerator.CreateNumber(1, 30));
        firstBook.Category = (BookCategory)RandomGenerator.CreateNumber(1, 12);
        firstBook.Finished = !firstBook.Finished;

        var result = await service.UpdateBookAsync(user.Id, firstBook);

        // Assert
        result.Should().BeTrue();
        
        var expected = Mapper.Map<Book, BookEntity>(firstBook);
        expected.UserId = user.Id;

        (await Context.Books.FirstAsync()).Should().BeEquivalentTo(expected,
            o => o.Excluding(n => n.Path.Equals("Sessions") ||
                                  n.Path.EndsWith("Created") ||
                                  n.Path.EndsWith("Modified")));
    }

    [Fact]
    public async Task CanDeleteBook()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();
        
        var service = new BookService(new Mock<ILogger<BookService>>().Object, Context, Mapper);

        // Act
        var result = await service.DeleteBookAsync(user.Id, books.First().Id);

        // Assert
        result.Should().BeTrue();

        (await Context.Books.FirstAsync()).IsDeleted.Should().BeTrue();
    }
}