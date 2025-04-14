using TodoApp.Application.Interfaces;
using System.Collections.Generic;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Infrastructure.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private List<string> categories = new List<string> { "Work", "Personal", "Study" };

    private readonly TodoDbContext _dbContext;
    public TodoListRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public int GetNextId()
    {
        return _dbContext.TodoItems.Any() ? _dbContext.TodoItems.Max(t => t.Id) + 1 : 1;
    }
    public List<string> GetAllCategories() => categories;
}
