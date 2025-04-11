namespace TodoApp.Application.Interfaces;

public interface ITodoListRepository
{
    int GetNextId();
    List<string> GetAllCategories();
}
