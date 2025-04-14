using System;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Domain.Tests.Entities;

public class ProgressionTest
{
     [Fact]
     public void Properties_ShouldBeSetCorrectly()
     {
          // Arrange
          var progression = new Progression
          {
               Id = 1,
               Date = new DateTime(2023, 1, 1),
               Percent = 75.5m,
               TodoItemId = 10
          };

          // Assert
          Assert.Equal(1, progression.Id);
          Assert.Equal(new DateTime(2023, 1, 1), progression.Date);
          Assert.Equal(75.5m, progression.Percent);
          Assert.Equal(10, progression.TodoItemId);
     }

     [Fact]
     public void DefaultConstructor_ShouldInitializePropertiesToDefaultValues()
     {
          // Arrange
          var progression = new Progression();

          // Assert
          Assert.Equal(0, progression.Id);
          Assert.Equal(default(DateTime), progression.Date);
          Assert.Equal(0m, progression.Percent);
          Assert.Equal(0, progression.TodoItemId);
     }
}