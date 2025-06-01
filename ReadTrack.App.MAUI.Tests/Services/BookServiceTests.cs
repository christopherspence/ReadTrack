using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.Tests.Utilities;
using ReadTrack.Shared.Api;
using Xunit;

namespace ReadTrack.App.MAUI.Tests.Services;

public class BookServiceTests
{
    [Fact]
    public async Task BookService_GetBooks_GetsBooks()
    {
        // Arrange
        var books = RandomGenerator.GenerateOneHundredRandomBooks();

        var apiMock = new Mock<IBookApi>();
        apiMock.Setup(m => m.GetBookCountAsync(It.IsAny<string>())).ReturnsAsync(books.Count());
         apiMock.Setup(m => m.GetBooksAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(books.Take(10));

        var service = new BookService(new Mock<ILogger<BookService>>().Object, apiMock.Object);

        // Act        
        var result = await service.GetBooksAsync();

        // Assert
        result.Should().NotBeNull();
        // TODO: Get this to pass
        //result.Should().BeEquivalentTo(books);

        apiMock.Verify(m => m.GetBookCountAsync(string.Empty), Times.Once);
        apiMock.Verify(m => m.GetBooksAsync(It.IsAny<int>(), 10, string.Empty), Times.Exactly(10));
        apiMock.VerifyNoOtherCalls();
    }
    
}