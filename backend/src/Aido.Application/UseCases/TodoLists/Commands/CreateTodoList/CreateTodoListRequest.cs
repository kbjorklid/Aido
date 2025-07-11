namespace Aido.Application.UseCases.TodoLists.Commands.CreateTodoList;

public class CreateTodoListRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public CreateTodoListRequest(string name, string description)
    {
        Name = name;
        Description = description;
    }
}