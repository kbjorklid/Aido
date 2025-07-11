using Aido.Core;

namespace Aido.Application.UseCases.TodoItems.Commands.AddTodoItem;

public class AddTodoItemRequest
{
    public TodoListId ListId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public AddTodoItemRequest(TodoListId listId, string title, string description)
    {
        ListId = listId;
        Title = title;
        Description = description;
    }
}