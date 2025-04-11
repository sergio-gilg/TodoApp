using MediatR;
using TodoApp.Domain.Entities;
using System.Collections.Generic;

namespace TodoApp.Application.Queries;

public class GetTodoItemProgressionsQuery : IRequest<List<Progression>>
{
    public int Id { get; set; }
}