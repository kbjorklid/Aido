using Aido.Core;

namespace Aido.Application.UseCases.TodoItems.Commands.IncompleteTodoItem;

public class IncompleteTodoItemRequest
{
    public TodoListId ListId { get; set; }
    public TodoItemId ItemId { get; set; }

    public IncompleteTodoItemRequest(TodoListId listId, TodoItemId itemId)
    {
        ListId = listId;
        ItemId = itemId;
    }
}