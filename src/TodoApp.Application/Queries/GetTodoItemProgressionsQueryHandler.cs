using MediatR;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Queries;

public class GetTodoItemProgressionsQueryHandler : IRequestHandler<GetTodoItemProgressionsQuery, List<Progression>>
{
    private readonly ITodoList _todoList;

    public GetTodoItemProgressionsQueryHandler(ITodoList todoList)
    {
        _todoList = todoList;
    }

    public async Task<List<Progression>> Handle(GetTodoItemProgressionsQuery request, CancellationToken cancellationToken)
    {
        var item = _todoList.GetItems().Find(i => i.Id == request.Id);
        return item?.Progressions ?? new List<Progression>();
    }
}