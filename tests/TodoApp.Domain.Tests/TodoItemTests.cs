using Xunit;
using TodoApp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace TodoApp.Domain.Tests;

public class TodoItemTests
{
    [Fact]
    public void IsCompleted_ReturnsTrue_WhenTotalPercentIs100()
    {
        var todoItem = new TodoItem
        {
            Progressions = new List<Progression>
            {
                new Progression { Date = DateTime.Now.AddDays(-2), Percent = 30 },
                new Progression { Date = DateTime.Now.AddDays(-1), Percent = 70 }
            }
        };

        Assert.True(todoItem.IsCompleted);
    }

    [Fact]
    public void IsCompleted_ReturnsFalse_WhenTotalPercentLessThan100()
    {
        var todoItem = new TodoItem
        {
            Progressions = new List<Progression>
            {
                new Progression { Date = DateTime.Now.AddDays(-2), Percent = 30 },
                new Progression { Date = DateTime.Now.AddDays(-1), Percent = 50 }
            }
        };

        Assert.False(todoItem.IsCompleted);
    }

    [Fact]
    public void AddProgression_AddsProgressionToList()
    {
        var todoItem = new TodoItem();
        var newProgression = new Progression { Date = DateTime.Now, Percent = 50 };
        todoItem.AddProgression(newProgression);
        Assert.Single(todoItem.Progressions);
        Assert.Contains(newProgression, todoItem.Progressions);
    }

    [Fact]
    public void AddProgression_ThrowsException_WhenProgressionDateIsInvalid()
    {
        var todoItem = new TodoItem();
        var newProgression = new Progression { Date = DateTime.Now.AddDays(-1), Percent = 50 };
        todoItem.AddProgression(newProgression);
        Assert.Throws<ArgumentException>(() => todoItem.AddProgression(new Progression { Date = DateTime.Now.AddDays(-2), Percent = 30 }));
    }

    [Fact]
    public void AddProgression_ThrowsException_WhenTotalPercentExceeds100()
    {
        var todoItem = new TodoItem();
        todoItem.AddProgression(new Progression { Date = DateTime.Now.AddDays(-1), Percent = 50 });
        Assert.Throws<ArgumentException>(() => todoItem.AddProgression(new Progression { Date = DateTime.Now, Percent = 60 }));
    }

    [Fact]
    public void AddProgression_ThrowsException_WhenPercentIsInvalid()
    {
        var todoItem = new TodoItem();

        Assert.Throws<ArgumentOutOfRangeException>(() => todoItem.AddProgression(new Progression { Date = DateTime.Now.AddDays(-1), Percent = 0 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => todoItem.AddProgression(new Progression { Date = DateTime.Now, Percent = 110 }));
    }
}