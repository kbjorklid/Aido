using Aido.Application.LlmAnalysis;
using Aido.Application.Repositories;
using Aido.Application.UseCases.TodoItems.Commands.AddTodoItem;
using Aido.Application.UseCases.TodoItems.Commands.RemoveTodoItem;
using Aido.Application.UseCases.TodoLists.Commands.CreateTodoList;
using Aido.Application.UseCases.TodoLists.Commands.DeleteTodoList;
using Aido.Application.UseCases.TodoLists.Commands.GenerateAiSuggestions;
using Aido.Application.UseCases.TodoLists.Queries.GetAllTodoLists;
using Aido.Application.UseCases.TodoLists.Queries.GetTodoListById;
using Aido.Infrastructure.LlmAnalysis;
using Aido.Infrastructure.Repositories;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Register repositories
builder.Services.AddSingleton<ITodoListRepository, InMemoryTodoListRepository>();

// Register Semantic Kernel
builder.Services.AddKernel()
    .AddOpenAIChatCompletion(
        modelId: builder.Configuration["LLM:ModelId"] ?? throw new InvalidOperationException("LLM ModelId not configured"),
        apiKey: builder.Configuration["LLM:ApiKey"] ?? throw new InvalidOperationException("LLM API key not configured"),
        endpoint: new Uri(builder.Configuration["LLM:EndpointUrl"] ?? throw new InvalidOperationException("LLM EndpointUrl not configured")));

// Register LLM analysis port
builder.Services.AddScoped<LlmAnalysisPort, LlmAnalysisAdapter>();

// Register use cases
builder.Services.AddScoped<GetAllTodoListsUseCase>();
builder.Services.AddScoped<GetTodoListByIdUseCase>();
builder.Services.AddScoped<CreateTodoListUseCase>();
builder.Services.AddScoped<DeleteTodoListUseCase>();
builder.Services.AddScoped<AddTodoItemUseCase>();
builder.Services.AddScoped<RemoveTodoItemUseCase>();
builder.Services.AddScoped<GenerateAiSuggestionsUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("DevelopmentCorsPolicy");

app.MapControllers();

app.Run();
