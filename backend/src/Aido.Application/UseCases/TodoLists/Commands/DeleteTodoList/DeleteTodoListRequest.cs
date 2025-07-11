using Aido.Core;

namespace Aido.Application.UseCases.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListRequest
{
    public TodoListId Id { get; set; }

    public DeleteTodoListRequest(TodoListId id)
    {
        Id = id;
    }
}