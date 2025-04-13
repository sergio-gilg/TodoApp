using MediatR;
using System.Collections.Generic;

namespace TodoApp.Application.Queries;

public class GetAllCategoriesQuery : IRequest<List<string>>
{
}