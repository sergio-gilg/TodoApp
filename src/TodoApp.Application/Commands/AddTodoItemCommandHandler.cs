using MediatR;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Commands;

public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand, Unit>
{
    private readonly ITodoListRepository _repository;
    private readonly ITodoList _todoList;

    public AddTodoItemCommandHandler(ITodoListRepository repository, ITodoList todoList)
    {
        _todoList = todoList;
        _repository = repository;
    }

    public async Task<Unit> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        var id = _repository.GetNextId();
        _todoList.AddItem(id, request.Title, request.Description, request.Category);
        return Unit.Value;

    }
}