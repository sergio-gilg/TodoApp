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
}