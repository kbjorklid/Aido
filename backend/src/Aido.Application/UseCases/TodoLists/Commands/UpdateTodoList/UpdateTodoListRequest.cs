using Aido.Core;

namespace Aido.Application.UseCases.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListRequest
{
    public TodoListId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public UpdateTodoListRequest(TodoListId id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}