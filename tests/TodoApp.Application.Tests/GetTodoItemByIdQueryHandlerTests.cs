using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Queries;

public class GetTodoItemByIdQueryHandlerTest
{
    private readonly Mock<ITodoList> _todoListMock;
    private readonly GetTodoItemByIdQueryHandler _handler;

    public GetTodoItemByIdQueryHandlerTest()
    {
        _todoListMock = new Mock<ITodoList>();
        _handler = new GetTodoItemByIdQueryHandler(_todoListMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectTodoItem_WhenItemExists()
    {
        // Arrange
        var todoItem = new TodoItem { Id = 1, Title = "Test Item" };
        _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem> { todoItem });
        var query = new GetTodoItemByIdQuery { Id = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Item", result.Title);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenItemDoesNotExist()
    {
        // Arrange
        _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem>());
        var query = new GetTodoItemByIdQuery { Id = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldNotThrowException_ForValidInputs()
    {
        // Arrange
        var query = new GetTodoItemByIdQuery { Id = 1 };

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(query, CancellationToken.None));
        Assert.Null(exception);
    }

    [Fact]
    public async Task Handle_ShouldNotThrowException_ForValidItems()
    {
        var mockRepo = new Mock<ITodoList>();
        var handler = new GetTodoItemByIdQueryHandler(mockRepo.Object);
        var query = new GetTodoItemByIdQuery { Id = 2 };

        var todoItems = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Test", Description = "Test", Category = "Test" },
            new TodoItem { Id = 2, Title = "Test2", Description = "Test2", Category = "Test2" }
        };

        mockRepo.Setup(repo => repo.GetItems()).Returns(todoItems);

        TodoItem result = null;
        await Record.ExceptionAsync(async () =>
        {
            result = await handler.Handle(query, CancellationToken.None);
        });

        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal("Test2", result.Title);
    }

}
