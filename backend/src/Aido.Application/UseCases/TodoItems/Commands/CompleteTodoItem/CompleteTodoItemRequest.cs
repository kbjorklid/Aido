using Aido.Core;

namespace Aido.Application.UseCases.TodoItems.Commands.CompleteTodoItem;

public class CompleteTodoItemRequest
{
    public TodoListId ListId { get; set; }
    public TodoItemId ItemId { get; set; }

    public CompleteTodoItemRequest(TodoListId listId, TodoItemId itemId)
    {
        ListId = listId;
        ItemId = itemId;
    }
}