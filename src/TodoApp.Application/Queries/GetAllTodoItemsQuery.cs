using MediatR;
using TodoApp.Domain.Entities;
using System.Collections.Generic;

namespace TodoApp.Application.Queries;

public class GetAllTodoItemsQuery : IRequest<List<TodoItem>>
{
}