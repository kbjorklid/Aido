using Aido.Application.Repositories;
using Aido.Application.UseCases.TodoItems.Commands.AddTodoItem;
using Aido.Application.UseCases.TodoLists.Commands.CreateTodoList;
using Aido.Application.UseCases.TodoLists.Queries.GetAllTodoLists;
using Aido.Application.UseCases.TodoLists.Queries.GetTodoListById;
using Aido.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories
builder.Services.AddSingleton<ITodoListRepository, InMemoryTodoListRepository>();

// Register use cases
builder.Services.AddScoped<GetAllTodoListsUseCase>();
builder.Services.AddScoped<GetTodoListByIdUseCase>();
builder.Services.AddScoped<CreateTodoListUseCase>();
builder.Services.AddScoped<AddTodoItemUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
