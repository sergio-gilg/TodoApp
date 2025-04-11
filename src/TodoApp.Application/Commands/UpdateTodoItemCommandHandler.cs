using MediatR;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands;

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, Unit>
{
    private readonly ITodoList _todoList;

    public UpdateTodoItemCommandHandler(ITodoList todoList)
    {
        _todoList = todoList;
    }

    public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        _todoList.UpdateItem(request.Id, request.Description);
        return Unit.Value;
    }
}