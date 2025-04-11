// filepath: src/TodoApp.Application/Commands/RegisterProgressionCommandHandlerTest.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Commands;
using TodoApp.Application.Interfaces;
using Xunit;

namespace TodoApp.Application.Tests.Commands
{
    public class RegisterProgressionCommandHandlerTest
    {
        private readonly Mock<ITodoList> _todoListMock;
        private readonly RegisterProgressionCommandHandler _handler;

        public RegisterProgressionCommandHandlerTest()
        {
            _todoListMock = new Mock<ITodoList>();
            _handler = new RegisterProgressionCommandHandler(_todoListMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallRegisterProgressionWithCorrectParameters()
        {
            // Arrange
            var command = new RegisterProgressionCommand
            {
                Id = 1,
                Date = DateTime.UtcNow,
                Percent = 50
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _todoListMock.Verify(t => t.RegisterProgression(command.Id, command.Date, command.Percent), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotThrowException()
        {
            // Arrange
            var command = new RegisterProgressionCommand
            {
                Id = 1,
                Date = DateTime.UtcNow,
                Percent = 50
            };

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));
            Assert.Null(exception);
        }

        [Fact]
        public async Task Handle_ShouldHandleEdgeCase_WhenPercentIsZero()
        {
            // Arrange
            var command = new RegisterProgressionCommand
            {
                Id = 1,
                Date = DateTime.UtcNow,
                Percent = 0
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _todoListMock.Verify(t => t.RegisterProgression(command.Id, command.Date, command.Percent), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldHandleEdgeCase_WhenPercentIsHundred()
        {
            // Arrange
            var command = new RegisterProgressionCommand
            {
                Id = 1,
                Date = DateTime.UtcNow,
                Percent = 100
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _todoListMock.Verify(t => t.RegisterProgression(command.Id, command.Date, command.Percent), Times.Once);
        }
    }
}