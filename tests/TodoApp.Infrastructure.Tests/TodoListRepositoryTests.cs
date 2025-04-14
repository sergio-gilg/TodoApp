using Xunit;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Application.Interfaces;
using System.Collections.Generic;
using TodoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

public class TodoListRepositoryTests
{
    private TodoDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListTestDb") // Unique database for each test
            .Options;

        return new TodoDbContext(options);
    }

    [Fact]
    public void GetNextId_ReturnsIncrementedId()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var repository = new TodoListRepository(context);
      
        // Act
        int firstId = repository.GetNextId();

         // Agregar un elemento inicial a la tabla TodoItems
        context.TodoItems.Add(new TodoItem { Id = 1, Title = "Test Item", Description = "Test Description", Category = "Work" });
        context.SaveChanges();
        
        int secondId = repository.GetNextId();

        // Assert
        Assert.Equal(1, firstId);
        Assert.Equal(2, secondId);
    }

    [Fact]
    public void GetAllCategories_ReturnsAllCategories()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var repository = new TodoListRepository(context);

        // Act
        List<string> categories = repository.GetAllCategories();

        // Assert
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
        var context = CreateInMemoryContext();
        var repository = new TodoListRepository(context);

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
        var context = CreateInMemoryContext();
        var repository = new TodoListRepository(context);

        // Act
        var categories = repository.GetAllCategories();

        // Assert
        Assert.NotEmpty(categories);
    }
}