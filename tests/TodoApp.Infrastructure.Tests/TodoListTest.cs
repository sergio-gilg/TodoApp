using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Infrastructure.Services.Tests;

public class TodoListTest
{
    private readonly Mock<ITodoListRepository> _repositoryMock;
    private readonly TodoList _todoList;

    public TodoListTest()
    {
        _repositoryMock = new Mock<ITodoListRepository>();
        _todoList = new TodoList(_repositoryMock.Object);

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
        var items = _todoList.GetItems();
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
        var item = _todoList.GetItems().First();
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
        var items = _todoList.GetItems();
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
        var item = _todoList.GetItems().First();
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
    public void RegisterProgression_ShouldThrowException_WhenDateIsInvalid()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");
        _todoList.RegisterProgression(1, DateTime.UtcNow, 50);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _todoList.RegisterProgression(1, DateTime.UtcNow.AddMinutes(-1), 10));
    }

    [Fact]
    public void RegisterProgression_ShouldThrowException_WhenTotalPercentExceeds100()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");
        _todoList.RegisterProgression(1, DateTime.UtcNow, 60);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _todoList.RegisterProgression(1, DateTime.UtcNow.AddMinutes(1), 50));
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

    [Fact]
    public void PrintItems_ShouldOutputCorrectFormat()
    {
        // Arrange
        _todoList.AddItem(1, "Test Title", "Test Description", "Work");
        _todoList.RegisterProgression(1, DateTime.UtcNow, 50);

        // Act & Assert
        // Redirect Console output to verify the output format
        using (var consoleOutput = new System.IO.StringWriter())
        {
            Console.SetOut(consoleOutput);
            _todoList.PrintItems();

            var output = consoleOutput.ToString();
            Assert.Contains("1) Test Title - Test Description (Work) Completed:False", output);
            Assert.Contains("50% |OOOOOOOOOOOOOOOOOOOOOOOOO                         |", output);
        }
    }

    [Fact]
    public void PrintItems_ShouldOutputCorrectFormatCompleted()
    {
        var mockRepo = new Mock<ITodoListRepository>();
        var todoList = new TodoList(mockRepo.Object);

        // Configurar el mock para devolver una lista de categorías válida
        mockRepo.Setup(repo => repo.GetAllCategories()).Returns(new List<string> { "Work" });

        todoList.AddItem(1, "Test Title", "Test Description", "Work");
        todoList.RegisterProgression(1, DateTime.Parse("2025-03-18"), 30);
        todoList.RegisterProgression(1, DateTime.Parse("2025-03-19"), 50);
        todoList.RegisterProgression(1, DateTime.Parse("2025-03-20"), 20);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        todoList.PrintItems();

        var output = consoleOutput.ToString();

        Assert.Contains("1) Test Title - Test Description (Work) Completed:True", output);
        Assert.Contains("3/18/2025 12:00:00 AM - 30% |OOOOOOOOOOOOOOO                                   |", output);
        Assert.Contains("3/19/2025 12:00:00 AM - 80% |OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO          |", output);
        Assert.Contains("3/20/2025 12:00:00 AM - 100% |OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO|", output);
    }

    [Fact]
    public void UpdateItem_ShouldThrowException_WhenItemDoesNotExist()
    {
        var mockRepo = new Mock<ITodoListRepository>();
        var todoList = new TodoList(mockRepo.Object);

        // Configurar el mock para devolver una lista de categorías válida
        mockRepo.Setup(repo => repo.GetAllCategories()).Returns(new List<string> { "TestCategory" });

        Assert.Throws<ArgumentException>(() => todoList.UpdateItem(1, "New Description"));
    }

    [Fact]
    public void RemoveItem_ShouldThrowException_WhenItemDoesNotExist()
    {
        var mockRepo = new Mock<ITodoListRepository>();
        var todoList = new TodoList(mockRepo.Object);

        // Configurar el mock para devolver una lista de categorías válida
        mockRepo.Setup(repo => repo.GetAllCategories()).Returns(new List<string> { "TestCategory" });

        Assert.Throws<ArgumentException>(() => todoList.RemoveItem(1));
    }
}
