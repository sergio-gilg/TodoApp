namespace TodoApp.Domain.Entities;

public class Progression
{
     public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Percent { get; set; }
     public int TodoItemId { get; set; } 
}