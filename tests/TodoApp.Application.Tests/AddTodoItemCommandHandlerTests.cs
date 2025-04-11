using Xunit;
using Moq;
using TodoApp.Application.Commands;
using TodoApp.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Tests.Commands;

public class AddTodoItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCallRepositoryGetNextId()
    {
        var mockRepo = new Mock<ITodoListRepository>();
        var mockTodoList = new Mock<ITodoList>();
        var handler = new AddTodoItemCommandHandler(mockRepo.Object, mockTodoList.Object);
        var command = new AddTodoItemCommand { Title = "Test", Description = "Test", Category = "Test" };

        await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(repo => repo.GetNextId(), Times.Once);
    }
}