using Aido.Core;

namespace Aido.Application.UseCases.TodoLists.Commands.CreateTodoList;

public class TodoListResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<TodoItemResponse> Items { get; set; } = Enumerable.Empty<TodoItemResponse>();

    public static TodoListResponse FromDomain(TodoList todoList)
    {
        return new TodoListResponse
        {
            Id = todoList.Id,
            Name = todoList.Name,
            Description = todoList.Description,
            CreatedAt = todoList.CreatedAt,
            UpdatedAt = todoList.UpdatedAt,
            Items = todoList.Items.Select(TodoItemResponse.FromDomain)
        };
    }
}

public class TodoItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static TodoItemResponse FromDomain(TodoItem todoItem)
    {
        return new TodoItemResponse
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            CreatedAt = todoItem.CreatedAt,
            UpdatedAt = todoItem.UpdatedAt
        };
    }
}