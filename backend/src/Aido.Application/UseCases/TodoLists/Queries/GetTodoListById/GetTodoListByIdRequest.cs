using Aido.Core;

namespace Aido.Application.UseCases.TodoLists.Queries.GetTodoListById;

public class GetTodoListByIdRequest
{
    public TodoListId Id { get; set; }

    public GetTodoListByIdRequest(TodoListId id)
    {
        Id = id;
    }
}