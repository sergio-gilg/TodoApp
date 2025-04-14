using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Services;
using MediatR;
using TodoApp.Application.Commands;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoList, TodoList>();

// Register MediatR
builder.Services.AddMediatR(typeof(TodoApp.Application.Queries.GetAllTodoItemsQuery).Assembly);

builder.Services.AddTransient<AddTodoItemCommandHandler>();
builder.Services.AddTransient<RemoveTodoItemCommandHandler>();
builder.Services.AddTransient<UpdateTodoItemCommandHandler>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Permitir peticiones desde cualquier origen 
                   .AllowAnyMethod()   // Permitir cualquier método HTTP (GET, POST, etc.)
                   .AllowAnyHeader();  // Permitir cualquier encabezado
        });
});

// Registrar el contexto de la base de datos
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite("Data Source=TodoApp.db"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Usar CORS
app.UseCors("AllowAll");

app.MapControllers();

app.Run();