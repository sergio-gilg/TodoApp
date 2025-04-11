// filepath: src/TodoApp.Application/Commands/PrintTodoItemsCommandHandlerTest.cs
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Commands;
using TodoApp.Application.Interfaces;
using Xunit;

namespace TodoApp.Application.Tests.Commands
{
    public class PrintTodoItemsCommandHandlerTest
    {
        private readonly Mock<ITodoList> _todoListMock;
        private readonly PrintTodoItemsCommandHandler _handler;

        public PrintTodoItemsCommandHandlerTest()
        {
            _todoListMock = new Mock<ITodoList>();
            _handler = new PrintTodoItemsCommandHandler(_todoListMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallPrintItems()
        {
            // Arrange
            var command = new PrintTodoItemsCommand();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _todoListMock.Verify(t => t.PrintItems(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotThrowException()
        {
            // Arrange
            var command = new PrintTodoItemsCommand();

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));
            Assert.Null(exception);
        }
    }
}