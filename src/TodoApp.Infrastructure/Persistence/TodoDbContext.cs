using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Progression> Progressions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoItem>()
            .HasKey(t => t.Id); 

        modelBuilder.Entity<Progression>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Progression>()
            .HasOne<TodoItem>()
            .WithMany(t => t.Progressions)
            .HasForeignKey(p => p.TodoItemId);      

       
    }
}