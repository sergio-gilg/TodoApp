using MediatR;
namespace TodoApp.Application.Commands;

public class UpdateTodoItemCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }

}
