using MediatR;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Queries;

public class GetAllTodoItemsQueryHandler : IRequestHandler<GetAllTodoItemsQuery, List<TodoItem>>
{
    private readonly ITodoList _todoList;

    public GetAllTodoItemsQueryHandler(ITodoList todoList)
    {
        _todoList = todoList;
    }

    public async Task<List<TodoItem>> Handle(GetAllTodoItemsQuery request, CancellationToken cancellationToken)
    {
        return _todoList.GetItems();
    }
}