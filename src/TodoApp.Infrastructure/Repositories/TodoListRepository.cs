using TodoApp.Application.Interfaces;
using System.Collections.Generic;   

namespace TodoApp.Infrastructure.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private int nextId = 1;
    private List<string> categories = new List<string> { "Work", "Personal", "Study" };

    public int GetNextId() => nextId++;
    public List<string> GetAllCategories() => categories;
}
