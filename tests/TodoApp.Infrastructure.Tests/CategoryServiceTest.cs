using System.Collections.Generic;
using Moq;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure.Services;
using Xunit;

namespace TodoApp.Infrastructure.Services.Tests;

public class CategoryServiceTest
{
    private readonly Mock<ITodoListRepository> _repositoryMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTest()
    {
        _repositoryMock = new Mock<ITodoListRepository>();
        _categoryService = new CategoryService(_repositoryMock.Object);
    }

    [Fact]
    public void GetAllCategories_ShouldCallRepositoryMethodOnce()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(new List<string> { "Work", "Personal" });

        // Act
        var categories = _categoryService.GetAllCategories();

        // Assert
        _repositoryMock.Verify(r => r.GetAllCategories(), Times.Once);
        Assert.NotNull(categories);
    }

    [Fact]
    public void GetAllCategories_ShouldReturnCorrectCategories()
    {
        // Arrange
        var expectedCategories = new List<string> { "Work", "Personal" };
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(expectedCategories);

        // Act
        var categories = _categoryService.GetAllCategories();

        // Assert
        Assert.Equal(expectedCategories, categories);
    }

    [Fact]
    public void GetAllCategories_ShouldReturnEmptyList_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(new List<string>());

        // Act
        var categories = _categoryService.GetAllCategories();

        // Assert
        Assert.Empty(categories);
    }
}