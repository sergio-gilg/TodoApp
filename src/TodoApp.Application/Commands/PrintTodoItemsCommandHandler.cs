using MediatR;
using TodoApp.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Commands;

public class PrintTodoItemsCommandHandler : IRequestHandler<PrintTodoItemsCommand, Unit>
{
    private readonly ITodoList _todoList;

    public PrintTodoItemsCommandHandler(ITodoList todoList)
    {
        _todoList = todoList;
    }

    public async Task<Unit> Handle(PrintTodoItemsCommand request, CancellationToken cancellationToken)
    {
        _todoList.PrintItems();
        return Unit.Value;
    }
}