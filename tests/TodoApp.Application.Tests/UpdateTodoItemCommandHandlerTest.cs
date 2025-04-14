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

     [Fact]
    public void Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Title = "Test Title",
            Description = "Test Description",
            Category = "Test Category"
        };

        // Assert
        Assert.Equal(1, command.Id);
        Assert.Equal("Test Title", command.Title);
        Assert.Equal("Test Description", command.Description);
        Assert.Equal("Test Category", command.Category);
    }

    [Fact]
    public void DefaultConstructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Arrange
        var command = new UpdateTodoItemCommand();

        // Assert
        Assert.Equal(0, command.Id);
        Assert.Null(command.Title);
        Assert.Null(command.Description);
        Assert.Null(command.Category);
    }
}