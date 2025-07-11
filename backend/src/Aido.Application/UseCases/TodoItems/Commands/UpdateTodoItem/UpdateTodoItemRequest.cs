using Aido.Core;

namespace Aido.Application.UseCases.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemRequest
{
    public TodoListId ListId { get; set; }
    public TodoItemId ItemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public UpdateTodoItemRequest(TodoListId listId, TodoItemId itemId, string title, string description)
    {
        ListId = listId;
        ItemId = itemId;
        Title = title;
        Description = description;
    }
}