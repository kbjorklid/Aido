using Aido.Core;

namespace Aido.Application.UseCases.TodoLists.Commands.GenerateAiSuggestions;

public class GenerateAiSuggestionsRequest
{
    public TodoListId TodoListId { get; }
    public int MaxSuggestions { get; }

    public GenerateAiSuggestionsRequest(TodoListId todoListId, int maxSuggestions = 3)
    {
        TodoListId = todoListId;
        MaxSuggestions = maxSuggestions;
    }
}