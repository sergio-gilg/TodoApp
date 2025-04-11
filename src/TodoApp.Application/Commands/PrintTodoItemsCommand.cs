using MediatR;

namespace TodoApp.Application.Commands;

public class PrintTodoItemsCommand : IRequest<Unit>
{
}