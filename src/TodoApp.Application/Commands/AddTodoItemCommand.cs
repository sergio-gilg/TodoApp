using MediatR;

namespace TodoApp.Application.Commands;

public class AddTodoItemCommand : IRequest<Unit>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}