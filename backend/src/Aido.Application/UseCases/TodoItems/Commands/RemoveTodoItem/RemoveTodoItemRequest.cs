using Aido.Core;

namespace Aido.Application.UseCases.TodoItems.Commands.RemoveTodoItem;

public class RemoveTodoItemRequest
{
    public TodoListId ListId { get; set; }
    public TodoItemId ItemId { get; set; }

    public RemoveTodoItemRequest(TodoListId listId, TodoItemId itemId)
    {
        ListId = listId;
        ItemId = itemId;
    }
}