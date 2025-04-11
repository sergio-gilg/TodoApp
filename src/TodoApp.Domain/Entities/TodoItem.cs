using TodoApp.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TodoApp.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public List<Progression> Progressions { get; set; } = new List<Progression>();

    public bool IsCompleted => Progressions.Sum(p => p.Percent) >= 100;

    public void AddProgression(Progression newProgression)
    {
        if (newProgression.Percent <= 0 || newProgression.Percent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(newProgression.Percent), "Percent is invalid.");
        }

        if (Progressions.Any() && newProgression.Date <= Progressions.Last().Date)
        {
            throw new ArgumentException("Date must be greater than previous.");
        }

        if (Progressions.Sum(p => p.Percent) + newProgression.Percent > 100)
        {
            throw new ArgumentException("Total percent exceed 100%.");
        }

        Progressions.Add(newProgression);
    }
}