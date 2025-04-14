using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Services;

public class TodoList : ITodoList
{
    private readonly TodoDbContext _dbContext;
    private readonly ITodoListRepository _repository;

    public TodoList(TodoDbContext dbContext, ITodoListRepository repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

    public void AddItem(int id, string title, string description, string category)
    {
        if (!_repository.GetAllCategories().Contains(category))
        {
            throw new ArgumentException("Category not found");
        }
        _dbContext.TodoItems.Add(new TodoItem { Id = id, Title = title, Description = description, Category = category, Progressions = new List<Progression>() });
        _dbContext.SaveChanges();
    }

    public void UpdateItem(int id, string description)
    {
        var item = _dbContext.TodoItems
        .Include(t=>t.Progressions)
        .FirstOrDefault(t => t.Id == id);

        if (item == null)
        {
            throw new ArgumentException($"TodoItem with Id {id} not found.");
        }
        if (item.Progressions == null || item.Progressions.Sum(i => i.Percent) > 50)
        {
            throw new InvalidOperationException("Item cannot be updated.");
        }
        item.Description = description;
        _dbContext.SaveChanges();
    }

    public void RemoveItem(int id)
    {
        var item = _dbContext.TodoItems
        .Include(t=>t.Progressions)
        .FirstOrDefault(t => t.Id == id);

        if (item == null)
        {
            throw new ArgumentException($"TodoItem with Id {id} not found.");
        }
        if (item.Progressions.Sum(i => i.Percent) > 50)
        {
            throw new InvalidOperationException("The item cannot be removed because its progress exceeds 50%.");
        }
        _dbContext.TodoItems.Remove(item);
        _dbContext.SaveChanges();
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
         var item = _dbContext.TodoItems
        .Include(t=>t.Progressions)
        .FirstOrDefault(t => t.Id == id);

        if (item == null)
        {
            throw new ArgumentException($"TodoItem with Id {id} not found.");
        }
        if (percent <= 0 || percent > 100)
        {
            throw new ArgumentOutOfRangeException("Percent is invalid");
        }

        if (item.Progressions.Any() && dateTime <= item.Progressions.Last().Date)
        {
            throw new ArgumentException("Date must be greater than previous.");
        }

        if (item.Progressions.Sum(p => p.Percent) + percent > 100)
        {
            throw new InvalidOperationException("Total percent exceed 100%.");
        }

        var progression = new Progression { Date = dateTime, Percent = percent, TodoItemId = id  };
        item.Progressions.Add(progression);
        _dbContext.SaveChanges();
    }

    public List<TodoItem> GetItems()
    {
          return _dbContext.TodoItems.Include(t => t.Progressions).OrderBy(i => i.Id).ToList();
    }

    public void PrintItems()
    {
        foreach (var item in _dbContext.TodoItems.Include(t => t.Progressions).OrderBy(i => i.Id))
        {
            Console.WriteLine($"{item.Id}) {item.Title} - {item.Description} ({item.Category}) Completed:{item.IsCompleted}");
            decimal totalPercent = 0;
            foreach (var progression in item.Progressions)
            {
                totalPercent += progression.Percent;
                string formattedDate = progression.Date.ToString(progression.Date.Month < 10 ? "M/dd/yyyy hh:mm:ss tt" : "MM/dd/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                Console.WriteLine($"{formattedDate} - {totalPercent}% {GetProgressBar(totalPercent)}");
            }
        }
    }

    private string GetProgressBar(decimal percent)
    {
        int barLength = 50;
        int filledLength = (int)(percent / 100 * barLength);
        return "|" + new string('O', filledLength) + new string(' ', barLength - filledLength) + "|";
    }
}