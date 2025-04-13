using MediatR;
using TodoApp.Application.Interfaces; // Asegúrate de tener este using
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Queries;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<string>>
{
    private readonly ITodoListRepository _repository;

    public GetAllCategoriesQueryHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<string>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAllCategories();
    }
}