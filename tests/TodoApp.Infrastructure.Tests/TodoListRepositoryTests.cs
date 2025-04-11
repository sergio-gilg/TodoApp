using Xunit;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Application.Interfaces;
using Moq;
using System.Collections.Generic;

public class TodoListRepositoryTests
{
    [Fact]
    public void GetNextId_ReturnsIncrementedId()
    {
        var repository = new TodoListRepository();
        int firstId = repository.GetNextId();
        int secondId = repository.GetNextId();

        Assert.Equal(1, firstId);
        Assert.Equal(2, secondId);
    }

    [Fact]
    public void GetAllCategories_ReturnsAllCategories()
    {
        var repository = new TodoListRepository();
        List<string> categories = repository.GetAllCategories();
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
        Assert.Contains("Work", categories);
        Assert.Contains("Personal", categories);
        Assert.Contains("Study", categories);
    }


    [Fact]
    public void GetAllCategories_ShouldReturnPredefinedCategories()
    {
        // Arrange
        var repository = new TodoListRepository();

        // Act
        var categories = repository.GetAllCategories();

        // Assert
        var expectedCategories = new List<string> { "Work", "Personal", "Study" };
        Assert.Equal(expectedCategories, categories);
    }

    [Fact]
    public void GetAllCategories_ShouldReturnNonEmptyList()
    {
        // Arrange
        var repository = new TodoListRepository();

        // Act
        var categories = repository.GetAllCategories();

        // Assert
        Assert.NotEmpty(categories);
    }
}