using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoApp.Infrastructure.Services;

public class TodoList : ITodoList
{
    private static List<TodoItem> items = new List<TodoItem>();
    private readonly ITodoListRepository _repository;

    public TodoList(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public void AddItem(int id, string title, string description, string category)
    {
        if (!_repository.GetAllCategories().Contains(category))
        {
            throw new ArgumentException("Category not found");
        }
        items.Add(new TodoItem { Id = id, Title = title, Description = description, Category = category, Progressions = new List<Progression>() });
    }

    public void UpdateItem(int id, string description)
    {
        var item = items.Find(i => i.Id == id);
        if (item == null) 
        {
            throw new ArgumentException($"TodoItem with Id {id} not found.");
        }
        if (item.Progressions == null || item.Progressions.Sum(i => i.Percent) > 50)
        {
            throw new InvalidOperationException("Item cannot be updated.");
        }
        item.Description = description;
    }

    public void RemoveItem(int id)
    {
        var item = items.Find(i => i.Id == id);
        if (item == null)
        {
            throw new ArgumentException($"TodoItem with Id {id} not found.");
        }
        if (item.Progressions.Sum(i => i.Percent) > 50)
        {
            throw new InvalidOperationException("Item cannot be removed.");
        }
        items.Remove(item);
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = items.Find(i => i.Id == id);
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
            throw new ArgumentException("Total percent exceed 100%.");
        }

        item.Progressions.Add(new Progression { Date = dateTime, Percent = percent });
    }

    public List<TodoItem> GetItems()
    {
        return items.OrderBy(i => i.Id).ToList();
    }

    public void PrintItems()
    {
        foreach (var item in items.OrderBy(i => i.Id))
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