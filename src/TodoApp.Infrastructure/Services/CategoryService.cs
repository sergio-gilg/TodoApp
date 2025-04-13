namespace TodoApp.Infrastructure.Services;

using TodoApp.Application.Interfaces;

public class CategoryService : ICategoryService
{
    private readonly ITodoListRepository _repository;

    public CategoryService(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public List<string> GetAllCategories()
    {
        return _repository.GetAllCategories();
    }
}