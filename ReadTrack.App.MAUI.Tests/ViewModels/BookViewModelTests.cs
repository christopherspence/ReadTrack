using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.Tests.Utilities;
using ReadTrack.App.MAUI.ViewModels;
using ReadTrack.Shared.Models;
using Xunit;

namespace ReadTrack.App.MAUI.Tests.ViewModels;

public class BookViewModelTests
{
    [Fact]
    public async Task BookViewModel_GetAsync_GetsBooks()
    {
        // Arrange
        var serviceMock = new Mock<IBookService>();
        var books = RandomGenerator.GenerateOneHundredRandomBooks();

        serviceMock.Setup(m => m.GetBooksAsync()).ReturnsAsync(books);

        var viewModel = new BookViewModel(serviceMock.Object);

        // Act
        var result = await viewModel.GetAsync();

        // Assert
        var expected = new ObservableCollection<Book>(books);
        result.Should().BeEquivalentTo(expected);
        viewModel.Books.Should().BeEquivalentTo(expected);

        serviceMock.Verify(m => m.GetBooksAsync(), Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }
}