using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Moq;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Persistence;
using Xunit;

namespace TodoApp.Infrastructure.Services.Tests;

public class TodoListTest
{
    private readonly Mock<ITodoListRepository> _repositoryMock;
    private readonly TodoList _todoList;
    private readonly TodoDbContext _context;

    private TodoDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database for each test
            .Options;

        return new TodoDbContext(options);
    }

    public TodoListTest()
    {
        _repositoryMock = new Mock<ITodoListRepository>();
        _context = CreateInMemoryContext();
        _todoList = new TodoList(_context, _repositoryMock.Object);
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(new List<string> { "Work", "Personal" });
    }

    [Fact]
    public void AddItem_ShouldAddItem_WhenCategoryIsValid()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(new List<string> { "Work", "Personal" });

        // Act
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");

        // Assert
        var items = _context.TodoItems.ToList();
        Assert.Single(items);
        Assert.Equal("Test Title", items[0].Title);
    }

    [Fact]
    public void AddItem_ShouldThrowException_WhenCategoryIsInvalid()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllCategories()).Returns(new List<string> { "Work", "Personal" });

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _todoList.AddItem(1, "Test Title", "Test Description", "InvalidCategory"));
    }

    [Fact]
    public void UpdateItem_ShouldUpdateDescription_WhenConditionsAreValid()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Old Description", "Work");

        // Act
        _todoList.UpdateItem(1, "New Description");

        // Assert
        var item = _context.TodoItems.First();
        Assert.Equal("New Description", item.Description);
    }

    [Fact]
    public void UpdateItem_ShouldThrowException_WhenProgressionExceeds50Percent()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Old Description", "Work");
        _todoList.RegisterProgression(1, DateTime.UtcNow, 60);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _todoList.UpdateItem(1, "New Description"));
    }

    [Fact]
    public void RemoveItem_ShouldRemoveItem_WhenConditionsAreValid()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");

        // Act
        _todoList.RemoveItem(1);

        // Assert
        var items = _context.TodoItems.ToList();
        Assert.Empty(items);
    }

    [Fact]
    public void RemoveItem_ShouldThrowException_WhenProgressionExceeds50Percent()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");
        _todoList.RegisterProgression(1, DateTime.UtcNow, 60);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _todoList.RemoveItem(1));
    }

    [Fact]
    public void RegisterProgression_ShouldAddProgression_WhenConditionsAreValid()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");

        // Act
        _todoList.RegisterProgression(1, DateTime.UtcNow, 50);

        // Assert
        var item = _context.TodoItems.Include(t => t.Progressions).First();
        Assert.Single(item.Progressions);
        Assert.Equal(50, item.Progressions.First().Percent);
    }

    [Fact]
    public void RegisterProgression_ShouldThrowException_WhenPercentIsInvalid()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _todoList.RegisterProgression(1, DateTime.UtcNow, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => _todoList.RegisterProgression(1, DateTime.UtcNow, 101));
    }

    [Fact]
    public void RegisterProgression_ShouldThrowException_WhenTotalPercentExceeds100()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");
        _todoList.RegisterProgression(1, DateTime.UtcNow, 60);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _todoList.RegisterProgression(1, DateTime.UtcNow.AddMinutes(1), 50));
    }

    [Fact]
    public void GetItems_ShouldReturnAllItems()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title 1", "Test Description 1", "Work");
        _todoList.AddItem(2, "Test Title 2", "Test Description 2", "Personal");

        // Act
        var items = _todoList.GetItems();

        // Assert
        Assert.Equal(2, items.Count);
    }
}