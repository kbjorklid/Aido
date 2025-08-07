namespace Aido.Application.UseCases.TodoLists.Commands.GenerateAiSuggestions;

public class GenerateAiSuggestionsResponse
{
    public int SuggestionsAdded { get; }
    public IReadOnlyList<string> AddedSuggestions { get; }

    public GenerateAiSuggestionsResponse(int suggestionsAdded, IReadOnlyList<string> addedSuggestions)
    {
        SuggestionsAdded = suggestionsAdded;
        AddedSuggestions = addedSuggestions;
    }
}