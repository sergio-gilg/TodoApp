using MediatR;
namespace TodoApp.Application.Commands;

public class RemoveTodoItemCommand : IRequest<Unit>
{
    public int Id { get; set; }
}