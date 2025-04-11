using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddScoped<ITodoListRepository, TodoListRepository>();
                services.AddScoped<ITodoList, TodoList>();
            })
            .Build();

        await RunApp(host.Services);
    }

    static async Task RunApp(IServiceProvider services)
    {
        using IServiceScope scope = services.CreateScope();
        ITodoList todoList = scope.ServiceProvider.GetRequiredService<ITodoList>();
        ITodoListRepository repository = scope.ServiceProvider.GetRequiredService<ITodoListRepository>();
        try
        {
            // Lógica de la aplicación de consola aquí
            int nextId = repository.GetNextId();
            todoList.AddItem(nextId, "Complete Console App", "Finish the console application logic", "Work");

            nextId = repository.GetNextId();
            todoList.AddItem(nextId, "Buy Groceries", "Get food for the week", "Personal");

            todoList.RegisterProgression(1, DateTime.Now.AddDays(-2), 20);
            todoList.RegisterProgression(1, DateTime.Now.AddDays(-1), 30);
            todoList.RegisterProgression(1, DateTime.Now, 50);

            todoList.RegisterProgression(2, DateTime.Now.AddDays(-1), 40);
            todoList.RegisterProgression(2, DateTime.Now, 60);

            todoList.PrintItems();

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("1. Add a new TodoItem");
                Console.WriteLine("2. Register progression for a TodoItem");
                Console.WriteLine("3. Print all TodoItems");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewTodoItem(todoList, repository);
                        break;
                    case "2":
                        RegisterProgressionForTodoItem(todoList);
                        break;
                    case "3":
                        todoList.PrintItems();
                        break;
                    case "4":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void AddNewTodoItem(ITodoList todoList, ITodoListRepository repository)
    {
        Console.WriteLine("\nEnter details for the new TodoItem:");
        Console.Write("Title: ");
        string title = Console.ReadLine();
        Console.Write("Description: ");
        string description = Console.ReadLine();
        Console.Write("Category: ");
        string category = Console.ReadLine();

        try
        {
            int nextId = repository.GetNextId();
            todoList.AddItem(nextId, title, description, category);
            Console.WriteLine("TodoItem added successfully.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error adding TodoItem: {ex.Message}");
        }
    }

    static void RegisterProgressionForTodoItem(ITodoList todoList)
    {
        Console.Write("\nEnter the Id of the TodoItem: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Enter the progression date (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.Write("Enter the progression percent: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal percent))
                {
                    try
                    {
                        todoList.RegisterProgression(id, date, percent);
                        Console.WriteLine("Progression registered successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error registering progression: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid percent. Please enter a valid decimal.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Id. Please enter a valid integer.");
        }
    }
}