using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Queries;

public class GetAllTodoItemsQueryHandlerTest
{
    private readonly Mock<ITodoList> _todoListMock;
    private readonly GetAllTodoItemsQueryHandler _handler;

    public GetAllTodoItemsQueryHandlerTest()
    {
        _todoListMock = new Mock<ITodoList>();
        _handler = new GetAllTodoItemsQueryHandler(_todoListMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallGetItemsOnce()
    {
        // Arrange
        _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem>());

        // Act
        await _handler.Handle(new GetAllTodoItemsQuery(), CancellationToken.None);

        // Assert
        _todoListMock.Verify(t => t.GetItems(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectTodoItems()
    {
        // Arrange
        var expectedItems = new List<TodoItem>
        {
            new TodoItem { Id = 1, Description = "Task 1" },
            new TodoItem { Id = 2, Description = "Task 2" }
        };
        _todoListMock.Setup(t => t.GetItems()).Returns(expectedItems);

        // Act
        var result = await _handler.Handle(new GetAllTodoItemsQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(expectedItems, result);
    }

    [Fact]
    public async Task Handle_ShouldNotThrowException()
    {
        // Arrange
        _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem>());

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(new GetAllTodoItemsQuery(), CancellationToken.None));
        Assert.Null(exception);
    }
}