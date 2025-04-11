using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Services;
using MediatR;
using System.Reflection;
using TodoApp.Application.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoList, TodoList>();

// Register MediatR
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddMediatR(Assembly.Load("TodoApp.Application"));
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