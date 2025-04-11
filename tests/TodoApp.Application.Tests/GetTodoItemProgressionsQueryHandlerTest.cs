// filepath: src/TodoApp.Application/Queries/GetTodoItemProgressionsQueryHandlerTest.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Queries
{
    public class GetTodoItemProgressionsQueryHandlerTest
    {
        private readonly Mock<ITodoList> _todoListMock;
        private readonly GetTodoItemProgressionsQueryHandler _handler;

        public GetTodoItemProgressionsQueryHandlerTest()
        {
            _todoListMock = new Mock<ITodoList>();
            _handler = new GetTodoItemProgressionsQueryHandler(_todoListMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProgressions_WhenItemExists()
        {
            // Arrange
            var progressions = new List<Progression>
            {
                new Progression { Date = System.DateTime.UtcNow, Percent = 50 }
            };
            var todoItem = new TodoItem { Id = 1, Progressions = progressions };
            _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem> { todoItem });
            var query = new GetTodoItemProgressionsQuery { Id = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(50, result[0].Percent);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenItemDoesNotExist()
        {
            // Arrange
            _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem>());
            var query = new GetTodoItemProgressionsQuery { Id = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenProgressionsAreNull()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Progressions = null };
            _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem> { todoItem });
            var query = new GetTodoItemProgressionsQuery { Id = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ShouldNotThrowException_WhenTodoListIsEmpty()
        {
            // Arrange
            _todoListMock.Setup(t => t.GetItems()).Returns(new List<TodoItem>());
            var query = new GetTodoItemProgressionsQuery { Id = 1 };

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _handler.Handle(query, CancellationToken.None));
            Assert.Null(exception);
        }
    }
}