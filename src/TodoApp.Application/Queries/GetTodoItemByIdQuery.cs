using MediatR;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Queries;

public class GetTodoItemByIdQuery : IRequest<TodoItem>
{
    public int Id { get; set; }
}