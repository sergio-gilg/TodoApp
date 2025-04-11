using MediatR;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Queries;

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItem>
{
    private readonly ITodoList _todoList;

    public GetTodoItemByIdQueryHandler(ITodoList todoList)
    {
        _todoList = todoList;
    }

    public async Task<TodoItem> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
         var items = _todoList.GetItems();
        if (items == null)
        {
            return null; 
        }
        return items.FirstOrDefault(i => i.Id == request.Id);
    }
}