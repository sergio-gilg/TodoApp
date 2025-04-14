using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries;
using Xunit;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly Mock<ITodoListRepository> _repositoryMock;
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _repositoryMock = new Mock<ITodoListRepository>();
        _handler = new GetAllCategoriesQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategories_WhenRepositoryReturnsCategories()
    {
        // Arrange
        var expectedCategories = new List<string> { "Work", "Personal", "Study" };
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(expectedCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategories.Count, result.Count);
        Assert.Equal(expectedCategories, result);
        _repositoryMock.Verify(r => r.GetAllCategories(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenRepositoryReturnsEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(new List<string>());

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _repositoryMock.Verify(r => r.GetAllCategories(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllCategories()).Throws(new System.Exception("Repository error"));

        var query = new GetAllCategoriesQuery();

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(query, CancellationToken.None));
        _repositoryMock.Verify(r => r.GetAllCategories(), Times.Once);
    }
}