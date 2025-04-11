using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Commands;
using TodoApp.Application.Interfaces;
using Xunit;

namespace TodoApp.Application.Tests.Commands
{
    public class RemoveTodoItemCommandHandlerTest
    {
        private readonly Mock<ITodoList> _todoListMock;
        private readonly RemoveTodoItemCommandHandler _handler;

        public RemoveTodoItemCommandHandlerTest()
        {
            _todoListMock = new Mock<ITodoList>();
            _handler = new RemoveTodoItemCommandHandler(_todoListMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallRemoveItemWithCorrectId()
        {
            // Arrange
            var command = new RemoveTodoItemCommand { Id = 1 };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _todoListMock.Verify(t => t.RemoveItem(1), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotThrowException()
        {
            // Arrange
            var command = new RemoveTodoItemCommand { Id = 1 };

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));
            Assert.Null(exception);
        }
    }
}