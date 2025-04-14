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
}