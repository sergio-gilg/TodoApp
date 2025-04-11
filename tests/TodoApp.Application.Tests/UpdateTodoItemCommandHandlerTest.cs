using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Commands;
using TodoApp.Application.Interfaces;
using Xunit;

namespace TodoApp.Application.Tests.Commands;

public class UpdateTodoItemCommandHandlerTest
{
    private readonly Mock<ITodoList> _todoListMock;
    private readonly UpdateTodoItemCommandHandler _handler;

    public UpdateTodoItemCommandHandlerTest()
    {
        _todoListMock = new Mock<ITodoList>();
        _handler = new UpdateTodoItemCommandHandler(_todoListMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallUpdateItemWithCorrectParameters()
    {
        // Arrange
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Description = "Updated Description"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _todoListMock.Verify(t => t.UpdateItem(1, "Updated Description"), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotThrowException()
    {
        // Arrange
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Description = "Updated Description"
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));
        Assert.Null(exception);
    }
}